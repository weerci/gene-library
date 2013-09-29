library ISAlliance;

{ Important note about DLL memory management: ShareMem must be the
  first unit in your library's USES clause AND your project's (select
  Project-View Source) USES clause if your DLL exports any procedures or
  functions that pass strings as parameters or function results. This
  applies to all strings passed to and from your DLL--even those that
  are nested in records and classes. ShareMem is the interface unit to
  the BORLNDMM.DLL shared memory manager, which must be deployed along
  with your DLL. To avoid using BORLNDMM.DLL, pass string information
  using PChar or ShortString parameters. }

uses
  Windows, TlHelp32, SysUtils;

function GetProcessId(pName: PChar): dword;
var
 Snap: dword;
 Process: TPROCESSENTRY32;
begin
  Result := 0;
  Snap := CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
  if Snap <> INVALID_HANDLE_VALUE then
     begin
      Process.dwSize := SizeOf(TPROCESSENTRY32);
      if Process32First(Snap, Process) then
         repeat
          if lstrcmpi(Process.szExeFile, pName) = 0 then
             begin
              Result := Process.th32ProcessID;
              CloseHandle(Snap);
              Exit;
             end;
         until not Process32Next(Snap, Process);
      Result := 0;
      CloseHandle(Snap);
     end;
end;

function ProcessTerminate(dwPID:Cardinal):Boolean;
var
 hToken:THandle;
 SeDebugNameValue:Int64;
 tkp:TOKEN_PRIVILEGES;
 ReturnLength:Cardinal;
 hProcess:THandle;
begin
 Result:=false;
 // Добавляем привилегию SeDebugPrivilege
 // Для начала получаем токен нашего процесса
 if not OpenProcessToken( GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES or TOKEN_QUERY, hToken ) then
 begin
   MessageBox(0, 'lpProcessName', 'lpProcessName', 1);
   exit;
 end;

 // Получаем LUID привилегии
 if not LookupPrivilegeValue( nil, 'SeDebugPrivilege', SeDebugNameValue )
  then begin
   CloseHandle(hToken);
   exit;
  end;

 tkp.PrivilegeCount:= 1;
 tkp.Privileges[0].Luid := SeDebugNameValue;
 tkp.Privileges[0].Attributes := SE_PRIVILEGE_ENABLED;

 // Добавляем привилегию к нашему процессу
 AdjustTokenPrivileges(hToken,false,tkp,SizeOf(tkp),tkp,ReturnLength);
 if GetLastError()<> ERROR_SUCCESS  then exit;

 // Завершаем процесс. Если у нас есть SeDebugPrivilege, то мы можем
 // завершить и системный процесс
 // Получаем дескриптор процесса для его завершения
 hProcess := OpenProcess(PROCESS_TERMINATE, FALSE, dwPID);
 if hProcess =0  then exit;
  // Завершаем процесс
   if not TerminateProcess(hProcess, DWORD(-1))
    then exit;
 CloseHandle( hProcess );

 // Удаляем привилегию
 tkp.Privileges[0].Attributes := 0;
 AdjustTokenPrivileges(hToken, FALSE, tkp, SizeOf(tkp), tkp, ReturnLength);
 if GetLastError() <>  ERROR_SUCCESS
  then exit;

 Result:=true;
end;

procedure KillProcess(lpProcessName: PChar); stdcall;
var
  iProcess: dword;
begin
  iProcess := GetProcessId(lpProcessName);
  if (iProcess <> 0) then
    ProcessTerminate(GetProcessId(lpProcessName));
end;

exports KillProcess;

begin
end.
