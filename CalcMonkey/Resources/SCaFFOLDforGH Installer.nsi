; CalcsInstaller.nsi
;
; This script remembers the directory, 
; has uninstall support and (optionally) installs start menu shortcuts.
;
; It will install example2.nsi into a directory that the user selects,

;--------------------------------

; The name of the installer
Name "Magma Works SCaFFOLD for Grasshopper"

; The file to write
OutFile "SCaFFOLDforGH.exe"

; The default installation directory
InstallDir "$APPDATA\Grasshopper\Libraries"

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\SCaFFOLDforGH" "Install_Dir"

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
Section "SCaFFOLD for GH (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "CalcMonkey.gha"
  File "BriefFiniteElementNet.dll"
  File "CalcCoreStandard.dll"
  File "CalcWindowsUtilities.dll"
  File "DocumentFormat.OpenXml.dll"
  File "InteractionDiagram3D.dll"
  File "libSkiaSharp.dll"
  File "MWGeometry.dll"  
  File "netDxf.netstandard.dll"  
  File "Newtonsoft.Json.dll"
  File "SkiaSharp.dll"
  File "StructuralDrawing2D.dll"
  File "WpfMath.dll"
  
  SetOutPath "$INSTDIR\Libraries"
  File /nonfatal /a /r "Libraries\"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\SCaFFOLDforGH "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SCaFFOLDforGH" "DisplayName" "SCaFFOLD for Windows"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SCaFFOLDforGH" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SCaFFOLDforGH" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SCaFFOLDforGH" "NoRepair" 1
  WriteUninstaller "$INSTDIR\uninstall.exe"
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SCaFFOLDforGH"
  DeleteRegKey HKLM SOFTWARE\SCaFFOLDforGH
  
  ; Remove files and uninstaller

  ; Remove directories used
  RMDir $INSTDIR\Libraries

SectionEnd
