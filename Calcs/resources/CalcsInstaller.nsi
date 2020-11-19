; CalcsInstaller.nsi
;
; This script remembers the directory, 
; has uninstall support and (optionally) installs start menu shortcuts.
;
; It will install example2.nsi into a directory that the user selects,

;--------------------------------

; The name of the installer
Name "Magma Works SCaFFOLD"

; The file to write
OutFile "SCaFFOLD.exe"

; The default installation directory
InstallDir "$PROGRAMFILES64\Magma Works\SCaFFOLD"

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\SCaFFOLDforWindows" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

; The stuff to install
Section "SCaFFOLD for Windows (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "Calcs.exe"
  ;File "BriefFiniteElementNet.dll"
  File "CalcCoreStandard.dll"
  File "DocumentFormat.OpenXml.dll"
  File "DynamicRelaxation.dll"
  File "HelixToolkit.dll"
  File "HelixToolkit.Wpf.dll"
  File "LiveCharts.dll"
  File "LiveCharts.Wpf.dll"
  File "netDxf.netstandard.dll"
  File "Newtonsoft.Json.dll"
  File "WindowsBase.dll"
  File "WpfMath.dll"
  File "SkiaSharp.dll"
  File "SkiaSharp.Views.Desktop.dll"
  File "SkiaSharp.Views.Gtk.dll"
  File "SkiaSharp.Views.WPF.dll"
  File "StructuralDrawing2D.dll"
  File "CalcWindowsUtilities.dll"
  File "libSkiaSharp.dll"
  File "MWGeometry.dll"
  ; File "InteractionDiagram3D.dll"
  File "MIConvexHull.dll"
  File "Triangle.dll"
  File "Microsoft.Xaml.Behaviors.dll"
  File "Calcs.exe.config"
  File "System.Numerics.Vectors.dll"
  
  SetOutPath "$INSTDIR\Libraries"
  File /nonfatal /a /r "Libraries\"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\SCaFFOLDforWindows "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SCaFFOLDforWindows" "DisplayName" "SCaFFOLD for Windows"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SCaFFOLDforWindows" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SCaFFOLDforWindows" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SCaFFOLDforWindows" "NoRepair" 1
  WriteUninstaller "$INSTDIR\uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\SCaFFOLD"
  CreateShortcut "$SMPROGRAMS\SCaFFOLD\SCaFFOLD.lnk" "$INSTDIR\Calcs.exe" "" "$INSTDIR\Calcs.exe" 0
  
SectionEnd

Section "Desktop Shortcut"

  CreateShortCut "$DESKTOP\SCaFFOLD.lnk" "$INSTDIR\Calcs.exe"
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SCaFFOLDforWindows"
  DeleteRegKey HKLM SOFTWARE\SCaFFOLDforWindows
  
  ; Remove files and uninstaller

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\SCaFFOLD\*.*"
  Delete "$DESKTOP\SCaFFOLD.lnk"

  ; Remove directories used
  RMDir "$SMPROGRAMS\SCaFFOLDforWindows"
  RMDir $INSTDIR
  RMDir $INSTDIR\data
  RMDir /r /REBOOTOK $INSTDIR
  RMDir /REBOOTOK $INSTDIR\DLLs

SectionEnd
