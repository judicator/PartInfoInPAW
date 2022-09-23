# Part info in PAW menu

Very small and simple mod for Kerbal Space Program.
Mod works in VAB/SPH and adds some useful information about part to right-click menu (PAW menu):
* part name (basically, part ID);
* part mod (GameData folder name);
* dry/wet mass;
* cost;
* entry cost;
* dry engine TWR ("dry means" TWR is only calculated for this specific part, not craft or stage). Of course, is shown only for engines.

By default, mod adds it's functionality to all in game parts.

For engines, mod also display formatted string of additional data about engine (the same you can see in part information screen in left panel in VAB/SPH). If part has more than one ModuleEngines (for example, multi-mode engine like RAPIER), information for first two ModuleEngines will be displayed.

Lastly, mod adds two buttons to PAW menu, which allows you to copy part name (part ID) or whole part config (in-game part representation in human-readable text) to clipboard.

Actually, one picture can tell more than a thousand words:
![PartInfoInPAW](https://i.imgur.com/ucCfDGe.png)


## Dependencies

* [Module manager (last version preferred)](https://github.com/sarbian/ModuleManager)


## Supported KSP versions

KSP 1.8.1 or newer is supported.


## Licensing

The MIT License (MIT)

Copyright (c) 2022 Alexander Rogov

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
