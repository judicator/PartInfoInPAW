using System;
using System.Linq;
using UnityEngine;

namespace PartInfoInPAW
{
	public class ModulePartInfoInPAW : PartModule
	{
		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Name", groupName = "partInfo", groupDisplayName = "Part info")]
		public string partName = "";

		[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = "Copy part name", active = true, groupName = "partInfo", groupDisplayName = "Part info")]
		public void CopyPartName()
		{
			GUIUtility.systemCopyBuffer = partName;
		}

		[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = "Copy part CFG node", active = true, groupName = "partInfo", groupDisplayName = "Part info")]
		public void CopyPartConfigNode()
		{
			GUIUtility.systemCopyBuffer = GetConfigNodeText();
		}

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Dry mass", groupName = "partInfo", groupDisplayName = "Part info")]
		public string partMass = "0 kg";

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Cost", guiFormat = "F0", groupName = "partInfo", groupDisplayName = "Part info")]
		public float partCost = 0.0f;

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Entry cost", groupName = "partInfo", groupDisplayName = "Part info")]
		public int partEntryCost = 0;

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Engine TWR", guiFormat = "F3", groupName = "partInfo", groupDisplayName = "Part info")]
		public float partTWR = 0.0f;

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Info", groupName = "engine1Info", groupDisplayName = "Engine #1 info")]
		public string engine1Info = "";

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Info", groupName = "engine2Info", groupDisplayName = "Engine #2 info")]
		public string engine2Info = "";

		protected ModuleEngines engine1;
		protected ModuleEngines engine2;

		protected bool InfoUpdated = false;
		protected bool ShowTWR = true;

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
				Fields["partMass"].guiName = "Dry mass";
				partMass = FormatMass(dryMass);
			}
			else
			{
				// Dry mass / wet mass
				Fields["partMass"].guiName = "Dry / wet mass";
				partMass = FormatMass(dryMass) + " / " + FormatMass(wetMass);
			}

			partCost = part.partInfo.cost + part.GetModuleCosts(part.partInfo.cost);
			partEntryCost = part.partInfo.entryCost;

			engines = part.GetComponents<ModuleEngines>();
			if (engines.Length <= 0)
			{
				ShowTWR = false;
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
				else {
					Fields["engine2Info"].guiActiveEditor = false;
				}
			}
			if (ShowTWR)
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
				result = (mass * 1000.0f).ToString("F0") + " kg";
			}
			else
			{
				result = mass.ToString("F3") + " t";
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

		private string GetConfigNodeText()
		{
			string node = "";
			try
			{
				ConfigNode cfg = GameDatabase.Instance.GetConfigNode(part.partInfo.partUrl);
				node = cfg.ToString();
				if (cfg != null && !cfg.HasValue("name") && (partName != ""))
				{
					// node.Replace($"PART{Environment.NewLine}" + "{" + $"{Environment.NewLine}", $"PART{Environment.NewLine}" + "{" + $"{Environment.NewLine}\tname = " + partName + $"{Environment.NewLine}");
					// node.Replace("PART\n{\n", "PART\n{" + $"\n\tname = {partName}\n");
					node = ReplaceFirstOccurrence(node, "{", "{" + $"{Environment.NewLine}\tname = {partName}");
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

		[KSPEvent]
        public void ModuleDataChanged(BaseEventDetails details)
        {
			InfoUpdated = false;
        }
	}
}
