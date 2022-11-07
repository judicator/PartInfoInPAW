# Part info in PAW menu

Mod for Kerbal Space Program intended for mod creators and advanced players.
Get useful information about part from PAW menu, copy part ID or full part CFG node to clipboard with one click.

## What does this mod do?

1. It displays part name (basically, part ID) and path to part CFG file in the parts list (in VAB/Hangar):
![PartInfoInPAW](https://i.imgur.com/x889rHz.png)
This feature is more or less identical to [PartInfo mod by linuxgurugamer](https://forum.kerbalspaceprogram.com/index.php?/topic/182040-*).

2. PartInfoInPAW also adds some information to PAW menu (right-click menu) for alls parts (works only in VAB/SPH).
    This information include:
    * part name (basically, part ID);
    * part mod (GameData folder name);
    * dry/wet mass;
    * cost;
    * entry cost;
    * dry engine TWR ("dry" means TWR is only calculated for this specific part, not craft or stage). And of course, it is shown only for engines.
    
    For engines, mod also display formatted string of additional data about engine (the same you can see in parts list in VAB/SPH). If part has more than one ModuleEngines (for example, multi-mode engine like RAPIER), information for first two ModuleEngines will be displayed.

    ![PartInfoInPAW](https://i.imgur.com/5Mj9Wdk.png)

3. Finally, mod adds two buttons to PAW menu (see screenshot above), which allows you to copy part name (part ID) or whole part config (in-game part representation in human-readable text) to clipboard.
This is my favorite feature, and I hope other players and modders will find it useful (as did I).

## Dependencies

* [Module manager (preferably last version)](https://forum.kerbalspaceprogram.com/index.php?/topic/50533-*)
Version 4.2.2 is bundled as part of download.

## Supported KSP versions

KSP 1.8.0 or newer is supported.

## Licensing

The MIT License (MIT)

Copyright (c) 2022 Alexander Rogov

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 


Bundled Module Manager is licensed under a "CC share-alike license". More information can be found [here](https://forum.kerbalspaceprogram.com/index.php?/topic/50533-*).
