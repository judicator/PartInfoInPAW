namespace PartInfoInPAW
{
	public class ModulePartInfoInPAW: PartModule
	{
		[KSPField(isPersistant = true)]
		public string originalPartName = "";

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Name", groupName = "partInfo", groupDisplayName = "Part info")]
		public string partName = "";

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Dry mass", groupName = "partInfo", groupDisplayName = "Part info", guiFormat = "F3", guiUnits = " t")]
		public float partMass = 0.0f;

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Cost", groupName = "partInfo", groupDisplayName = "Part info")]
		public float partCost = 0.0f;

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Entry cost", groupName = "partInfo", groupDisplayName = "Part info")]
		public int partEntryCost = 0;

		[KSPField(isPersistant = false, guiActiveEditor = true, guiActive = false, guiName = "Engine TWR", groupName = "partInfo", groupDisplayName = "Part info", guiFormat = "F2")]
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

		public void UpdateInfo()
		{
			ModuleEngines[] engines;
			MultiModeEngine[] isMultimode;
			float totalThrust = 0.0f;

			if (originalPartName != "")
			{
				partName = originalPartName;
			}
			else
			{
				partName = part.partInfo.name;
			}
			partMass = part.mass;
			partCost = part.partInfo.cost;
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
				if (partMass > 0)
				{
					partTWR = totalThrust / (partMass * 9.81f);
				}
				Fields["partTWR"].guiActiveEditor = true;
			}
			else
			{
				Fields["partTWR"].guiActiveEditor = false;
			}
			InfoUpdated = true;
		}

		[KSPEvent]
        public void ModuleDataChanged(BaseEventDetails details)
        {
			InfoUpdated = false;
        }
	}
}
