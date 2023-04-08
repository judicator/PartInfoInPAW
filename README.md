# Part info in PAW menu

Mod for Kerbal Space Program intended for mod creators and advanced players.
Get useful information about part from PAW menu, copy part ID or full part CFG node to clipboard with one click.

## What does this mod do?

1. It displays part name (basically, part ID) and path to part CFG file in the parts list (in VAB/Hangar):
![PartInfoInPAW](https://i.imgur.com/x889rHz.png)
This feature is more or less identical to [PartInfo mod by linuxgurugamer](https://forum.kerbalspaceprogram.com/index.php?/topic/182040-*).

2. PartInfoInPAW also adds some information to PAW menu (right-click menu) for alls parts (works only in VAB/SPH, optionally some info is also available).
    This information include:
    * part name (basically, part ID) (shown only in VAB/SPH);
    * part mod (GameData folder name) (shown only in VAB/SPH);
    * dry/wet mass (shown only in VAB/SPH);
    * cost (shown only in VAB/SPH);
    * entry cost (shown only in VAB/SPH);
    * dry engine TWR ("dry" means TWR is only calculated for this specific part, not craft or stage). And of course, it is shown only for engines (shown only in VAB/SPH);
    * current and maximum part temperature and skin part temperature (shown only in flight scene, and only if showInfoInFlight param is set to true (see Parameters section).
    
    For engines, mod also display formatted string of additional data about engine (the same you can see in parts list in VAB/SPH). If part has more than one ModuleEngines (for example, multi-mode engine like RAPIER), information for first two ModuleEngines will be displayed.

    ![PartInfoInPAW](https://i.imgur.com/5Mj9Wdk.png)

3. Finally, mod adds two buttons to PAW menu (see screenshot above), which allows you to copy part name (part ID) or whole part config (in-game part representation in human-readable text) to clipboard.
This is my favorite feature, and I hope other players and modders will find it useful (as did I).
NB: **button "Copy part CFG node" copies actual in-game part representation, not text from original part CFG-file. That means, whole PART{} node with all MM-patches already applied to it.**


## Parameters

There are three boolean (true/false) parameters for ModulePartInfoInPAW.
You could change them in GameData/PartInfoInPAW/PartInfoInPAW.cfg patch or write yoor own patch for specific parts.

### showInfoInFlight
Default: false
If set to true, part temperature and skin temperature will be shown in PAW menu in flight, as well as "Copy Part ID to Clipboard" and "Copy Part CFG to Clipboard" buttons.

### showTWR
Default: true
If true, dry TWR will be shown for engines in PAW menu (VAB/SPH).

### showGetInfo
Default: true
If true, part ID and full path to parg CFG file will be shown in right-click menu in part list (VAB/SPH).

## Dependencies

* [Module manager (preferably last version)](https://forum.kerbalspaceprogram.com/index.php?/topic/50533-*)
Version 4.2.2 is bundled as part of download.

## Supported KSP versions

KSP 1.8.0 or newer is supported.

## Licensing

The MIT License (MIT)

Copyright (c) 2023 Alexander Rogov

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 


Bundled Module Manager is licensed under a "CC share-alike license". More information can be found [here](https://forum.kerbalspaceprogram.com/index.php?/topic/50533-*).
