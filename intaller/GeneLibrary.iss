; Скрипт создан через Мастер Inno Setup Script.
; ИСПОЛЬЗУЙТЕ ДОКУМЕНТАЦИЮ ДЛЯ ПОДРОБНОСТЕЙ ИСПОЛЬЗОВАНИЯ INNO SETUP!

#define MyAppName "GeneLibrary"
#define MyAppVersion "1.32"
#define MyAppPublisher "CoreLab"
#define MyAppURL "https://googledrive.com/host/0B49CBXu70uZAbmlid0NYaGxwQ0k/"
#define MyAppExeName "GeneLibrary.exe"

[Setup]
WizardSmallImageFile=biotechnologii.bmp
WizardImageFile=A4dna.bmp
; Примечание: Значение AppId идентифицирует это приложение.
; Не используйте одно и тоже значение в разных установках.
; (Для генерации значения GUID, нажмите Инструменты | Генерация GUID.)
AppId={{8A447213-B5CC-43C7-959C-B0CC0B39DDE5}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\GeneLibrary
DefaultGroupName=GeneLibrary
OutputDir=D:\datadi\programming\exe\gl
OutputBaseFilename=GeneLibrarySetup132
SetupIconFile=D:\datadi\programming\src\GeneLibrary\gene-library\res\16.ico
Compression=lzma
SolidCompression=yes
AppMutex=23E0E3CB-4A33-4A23-AA40-8501D5E16E08

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; 
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; 

[Files]
Source: ISAlliance.dll; DestDir: {app};
Source: "D:\datadi\programming\exe\gl\GeneLibrary.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\datadi\programming\exe\gl\GeneLibrary.chm"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\datadi\programming\exe\gl\GeneLibrary.ver"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\datadi\programming\exe\gl\GeneLibrary.vshost.exe.manifest"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\datadi\programming\exe\gl\MdiClientController.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\datadi\programming\exe\gl\Microsoft.Office.Interop.Excel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\datadi\programming\exe\gl\VS2005ToolBox.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\datadi\programming\src\GeneLibrary\gene-library\Model\patch\base.pdc"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\datadi\programming\src\GeneLibrary\gene-library\Model\patch\prk_card.pdc"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\datadi\programming\src\GeneLibrary\gene-library\Model\patch\prk_tab.pdc"; DestDir: "{app}"; Flags: ignoreversion
; Примечание: Не используйте "Flags: ignoreversion" для системных файлов

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
;Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
//Filename: "dummy"; Description: "Загружать программу при старте ОС"; Flags: postinstall nowait skipifdoesntexist

[ISFormDesigner]
WizardForm=FF0A005457495A415244464F524D0030101D04000054504630F10B5457697A617264466F726D0A57697A617264466F726D0C436C69656E744865696768740368010B436C69656E74576964746803F1010C4578706C696369744C65667402000B4578706C69636974546F7002000D4578706C6963697457696474680301020E4578706C69636974486569676874038E010D506978656C73506572496E636802600A54657874486569676874020D00F10C544E65774E6F7465626F6F6B0D4F757465724E6F7465626F6F6B00F110544E65774E6F7465626F6F6B506167650B57656C636F6D65506167650D4578706C69636974576964746803F1010E4578706C6963697448656967687403390100F10C544269746D6170496D6167651157697A6172644269746D6170496D61676505576964746803B10006486569676874033601094261636B436F6C6F720708636C57696E646F770D4578706C69636974576964746803B1000E4578706C696369744865696768740336010000F10E544E6577537461746963546578740D57656C636F6D654C6162656C32044C65667403B8000C4578706C696369744C65667403B800000000F110544E65774E6F7465626F6F6B5061676509496E6E6572506167650D4578706C69636974576964746803F1010E4578706C6963697448656967687403390100F10C544E65774E6F7465626F6F6B0D496E6E65724E6F7465626F6F6B00F110544E65774E6F7465626F6F6B506167650B4C6963656E7365506167650D4578706C69636974576964746803A1010E4578706C6963697448656967687403ED000000F110544E65774E6F7465626F6F6B506167650D53656C656374446972506167650D4578706C69636974576964746803A1010E4578706C6963697448656967687403ED000000F110544E65774E6F7465626F6F6B506167651653656C65637450726F6772616D47726F7570506167650D4578706C69636974576964746803A1010E4578706C6963697448656967687403ED000000F110544E65774E6F7465626F6F6B506167650F53656C6563745461736B73506167650D4578706C69636974576964746803A1010E4578706C6963697448656967687403ED000000F110544E65774E6F7465626F6F6B50616765095265616479506167650D4578706C69636974576964746803A1010E4578706C6963697448656967687403ED000000F110544E65774E6F7465626F6F6B506167650E496E7374616C6C696E67506167650D4578706C69636974576964746803A1010E4578706C6963697448656967687403ED0000000000F110544E65774E6F7465626F6F6B506167650C46696E6973686564506167650D4578706C69636974576964746803F1010E4578706C6963697448656967687403390100F10C544269746D6170496D6167651257697A6172644269746D6170496D61676532094261636B436F6C6F720708636C57696E646F770000000000

[UninstallDelete]
Name: "{app}\ISAlliance.dll"; Type: files;

[Code]
procedure KillProc(lpProcName: AnsiString); external 'KillProcess@{app}\ISAlliance.dll stdcall delayload uninstallonly';

[Code]
{ RedesignWizardFormBegin } // Не удалять эту строку!
// Не изменять эту секцию. Она создана автоматически.
procedure RedesignWizardForm;
begin
  with WizardForm.WizardBitmapImage do
  begin
    Width := ScaleX(177);
    Height := ScaleY(310);
    BackColor := clWindow;
  end;

  with WizardForm.WelcomeLabel2 do
  begin
    Left := ScaleX(184);
  end;

  with WizardForm.WizardBitmapImage2 do
  begin
    BackColor := clWindow;
  end;

{ ReservationBegin }
  // Вы можете добавить ваш код здесь.

{ ReservationEnd }
end;
// Не изменять эту секцию. Она создана автоматически.
{ RedesignWizardFormEnd } // Не удалять эту строку!

procedure InitializeWizard();
begin
  RedesignWizardForm;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var r: Integer;
begin
    if CurStep = ssInstall then
    begin
        Exec(ExpandConstant('{app}\unins000.exe'), '/VERYSILENT /SUPPRESSMSGBOXES', '', SW_SHOW, ewWaitUntilTerminated, r);
        // Дожидаемся завершения удаления...
        while CheckForMutexes('{#MyAppName}Mutex') do Sleep(100);
    end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then
  begin
    UnloadDLL(ExpandConstant('{app}\ISAlliance.dll'));
  end;
    if CurUninstallStep = usDone then
        DeleteFile(ExpandConstant('{userstartup}\{#MyAppName}.lnk'));
end;

function InitializeUninstall(): Boolean;
begin
  KillProc('{#MyAppExeName}');
  Result := true;
end;
