using KSP.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PartInfoInPAW
{
	public class ModulePartInfoInPAW : PartModule
	{
		[KSPField(isPersistant = true)]
		public bool showTWR = true;

		[KSPField(isPersistant = true)]
		public bool showGetInfo = true;

		[KSPField(isPersistant = true)]
		public bool showCfgPathInPAW = false;

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "#LOC_PartInfoInPAW_PartName_Title", groupName = "partInfo", groupDisplayName = "#LOC_PartInfoInPAW_PartInfo_GroupTitle")]
		public string partName = "";

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "#LOC_PartInfoInPAW_PartMod_Title", groupName = "partInfo", groupDisplayName = "#LOC_PartInfoInPAW_PartInfo_GroupTitle")]
		public string partMod = "";

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "#LOC_PartInfoInPAW_PartDryMass_Title", groupName = "partInfo", groupDisplayName = "#LOC_PartInfoInPAW_PartInfo_GroupTitle")]
		public string partMass = "0 kg";

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "#LOC_PartInfoInPAW_PartCost_Title", guiFormat = "F0", groupName = "partInfo", groupDisplayName = "#LOC_PartInfoInPAW_PartInfo_GroupTitle")]
		public float partCost = 0.0f;

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "#LOC_PartInfoInPAW_PartEntryCost_Title", groupName = "partInfo", groupDisplayName = "#LOC_PartInfoInPAW_PartInfo_GroupTitle")]
		public int partEntryCost = 0;

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "#LOC_PartInfoInPAW_PartEngineTWR_Title", guiFormat = "F3", groupName = "partInfo", groupDisplayName = "#LOC_PartInfoInPAW_PartInfo_GroupTitle")]
		public float partTWR = 0.0f;

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "#LOC_PartInfoInPAW_Info_Title", groupName = "engine1Info", groupDisplayName = "#LOC_PartInfoInPAW_Engine1Info_GroupTitle")]
		public string engine1Info = "";

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "#LOC_PartInfoInPAW_Info_Title", groupName = "engine2Info", groupDisplayName = "#LOC_PartInfoInPAW_Engine2Info_GroupTitle")]
		public string engine2Info = "";

		[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = "#LOC_PartInfoInPAW_CopyPartName_Action", active = true, groupName = "partInfo", groupDisplayName = "#LOC_PartInfoInPAW_PartInfo_GroupTitle")]
		public void CopyPartName()
		{
			GUIUtility.systemCopyBuffer = partName;
			Debug.Log(String.Format($"[PartInfoInPAW] Part {part.partInfo.name} : ID copied to clipboard"));
		}

		[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = "#LOC_PartInfoInPAW_CopyPartNode_Action", active = true, groupName = "partInfo", groupDisplayName = "#LOC_PartInfoInPAW_PartInfo_GroupTitle")]
		public void CopyPartConfigNode()
		{
			GUIUtility.systemCopyBuffer = GetConfigNodeText();
		}

		protected ModuleEngines engine1;
		protected ModuleEngines engine2;

		public struct ModWithComplexFoldersStruct
		{
			private string ModFolder;
			private int NestingLevel;

			public ModWithComplexFoldersStruct(string folder, int nestLevel = 2)
			{
				ModFolder = folder;
				NestingLevel = nestLevel;
			}

			public bool URLMatches(string url)
			{
				return (ModFolder == url.Split('/')[0]);
			}

			public string BuildModName(string url)
			{
				int nestLevel = NestingLevel;
				string[] folders = url.Split('/');
				string result = "";
				if (folders.Length < nestLevel)
				{
					nestLevel = folders.Length;
				}
				for (int i = 0; i < nestLevel; i++)
				{
					if (i > 0)
					{
						result += "/" + folders[i];
					}
					else
					{
						result += folders[i];
					}
				}
				return result;
			}
		}

		protected List<ModWithComplexFoldersStruct> ModsWithComplexFoldersStruct = new List<ModWithComplexFoldersStruct>()
		{
			new ModWithComplexFoldersStruct("SquadExpansion"),
			new ModWithComplexFoldersStruct("UmbraSpaceIndustries"),
			new ModWithComplexFoldersStruct("WildBlueIndustries"),
			new ModWithComplexFoldersStruct("Bluedog_DB", 3),
		};

		protected bool InfoUpdated = false;

		private void Start()
		{
			GameEvents.onEditorShipModified.Add(EditorShipModified);
		}

		private void OnDestroy()
		{
			GameEvents.onEditorShipModified.Remove(EditorShipModified);
		}

		private void EditorShipModified(ShipConstruct construct)
		{
			InfoUpdated = false;
		}

		public void Update()
		{
			if (!InfoUpdated && HighLogic.LoadedSceneIsEditor)
			{
				UpdateInfo();
			}
		}

		private void UpdateInfo()
		{
			if (partName == "")
			{
				partName = GetPartName();
			}
			if (partMod == "")
			{
				partMod = GetPartMod();
			}
			ModuleEngines[] engines;
			MultiModeEngine[] isMultimode;
			float totalThrust = 0.0f;

			float prefabMass = part.partInfo.partPrefab.mass;
			float dryMass = prefabMass + part.GetModuleMass(prefabMass);
			float resMass = part.GetResourceMass();
			float wetMass = dryMass + resMass;
			if (Math.Abs(resMass) <= float.Epsilon)
			{
				// Dry mass only
				Fields["partMass"].guiName = Localizer.Format("#LOC_PartInfoInPAW_PartDryMass_Title");
				partMass = FormatMass(dryMass);
			}
			else
			{
				// Dry mass / wet mass
				Fields["partMass"].guiName = Localizer.Format("#LOC_PartInfoInPAW_PartDryWetMass_Title");
				partMass = FormatMass(dryMass) + " / " + FormatMass(wetMass);
			}

			partCost = part.partInfo.cost + part.GetModuleCosts(part.partInfo.cost);
			partEntryCost = part.partInfo.entryCost;

			engines = part.GetComponents<ModuleEngines>();
			if (engines.Length <= 0)
			{
				showTWR = false;
				Fields["engine1Info"].guiActiveEditor = false;
				Fields["engine2Info"].guiActiveEditor = false;
			}
			if (engines.Length > 0)
			{
				engine1 = engines[0];
				totalThrust = engine1.GetMaxThrust();
				engine1Info = "<br>" + engine1.GetInfo();
				Fields["engine1Info"].guiActiveEditor = true;
				if (engines.Length > 1)
				{
					engine2 = engines[1];
					engine2Info = "<br>" + engine2.GetInfo();
					Fields["engine2Info"].guiActiveEditor = true;
					isMultimode = part.GetComponents<MultiModeEngine>();
					if (isMultimode.Length <= 0)
					{
						totalThrust += engine2.GetMaxThrust();
					}
				}
				else
				{
					Fields["engine2Info"].guiActiveEditor = false;
				}
			}
			if (showTWR)
			{
				partTWR = 0.0f;
				if (wetMass > 0)
				{
					partTWR = totalThrust / (wetMass * 9.81f);
				}
				Fields["partTWR"].guiActiveEditor = true;
			}
			else
			{
				Fields["partTWR"].guiActiveEditor = false;
			}
			InfoUpdated = true;
		}

		private string FormatMass(float mass)
		{
			string result;
			if (mass < 1.0f)
			{
				result = (mass * 1000.0f).ToString("F0") + " " + Localizer.Format("#LOC_PartInfoInPAW_Kg_Unit");
			}
			else
			{
				result = mass.ToString("F3") + " " + Localizer.Format("#LOC_PartInfoInPAW_T_Unit");
			}
			return result;
		}

		private string GetPartName()
		{
			string pName = "";
			try
			{
				pName = GameDatabase.Instance.GetConfigs("PART").
					Single(c => part.partInfo.name.Replace('_', '.') == c.name.Replace('_', '.')).name;
			}
			catch (Exception)
			{
				Debug.LogError(String.Format($"[PartInfoInPAW] Couldn't get config value name for part {part.partInfo.name}"));
			}
			return pName;
		}

		private string GetPartMod()
		{
			string url = part.partInfo.partUrl;
			foreach (var mod in ModsWithComplexFoldersStruct)
			{
				if (mod.URLMatches(url))
				{
					return "\n" + mod.BuildModName(url);
				}
			}
			return url.Split('/')[0];
		}

		private string GetConfigNodeText()
		{
			string node = "";
			try
			{
				ConfigNode cfg = GameDatabase.Instance.GetConfigNode(part.partInfo.partUrl);
				if (cfg == null)
				{
					cfg = part.partInfo.partConfig;
				}
				if (cfg != null)
				{
					node = cfg.ToString();
					if (cfg != null && !cfg.HasValue("name") && (partName != ""))
					{
						node = ReplaceFirstOccurrence(node, "{", "{" + $"{Environment.NewLine}\tname = {partName}");
						Debug.Log(String.Format($"[PartInfoInPAW] Part {part.partInfo.name} : CFG node copied to clipboard"));
					}
				}
				else
				{
					Debug.LogError(String.Format($"[PartInfoInPAW] Couldn't get config node for part {part.partInfo.name}"));
				}
			}
			catch (Exception)
			{
				Debug.LogError(String.Format($"[PartInfoInPAW] Couldn't get config node for part {part.partInfo.name}"));
			}
			return node;
		}

		public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
		{
			int Place = Source.IndexOf(Find);
			string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
			return result;
		}

		public override string GetInfo()
		{
			if (!showGetInfo)
			{
				return "";
			}
			if (partName == "")
			{
				partName = GetPartName();
			}
			string[] urlSegments = part.partInfo.partUrl.Split('/');
			if (urlSegments.Length > 1)
			{
				Array.Resize(ref urlSegments, urlSegments.Length - 1);
			}
			string partURL = String.Join("<color=#a0a0a0>/</color><br>", urlSegments) + ".cfg";
			return Localizer.Format("#LOC_PartInfoInPAW_PartModuleInfo", partName, partURL);
		}

		[KSPEvent]
		public void ModuleDataChanged(BaseEventDetails details)
		{
			InfoUpdated = false;
		}
	}
}
