# H1 SCaFFOLD
*Structural Calculations Framework*
Free, Open and Lightweight Design

<img src="Calcs/resources/CalcSplash.gif" width="300">

SCaFFOLD is a framework designed to make it easy for engineers to write calculations in code once and to have access to those calculations wherever they need them - whether that's accessing them through a Windows app, mobile apps or embedded within CAD or analysis packages. SCaFFOLD is open source and made available under the MIT license.

Any project using the framework will need to reference the CalcCore library. Calculations can be supplied by any library containing classes that implement the ICalc interface from CalcCore.

The repository contains a number of different projects including:
CalcCore - the core classes that enable the framework.
Calcs - Windows desktop application (WPF).
DynamicRelaxation - dynamic relaxation engine, used for animation in Calcs splash screen.
RunInConsole - console app client for CalcCore.
TestCalcs - various calcuations written for the framework.
