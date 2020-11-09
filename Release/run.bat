@echo off 
start /wait Excel2Lua.exe
@echo run copy config lua to LuaScirpts

for /r .\Lua %%i in (*.lua) do ( 
    echo %%i 
    copy /y %%i ..\TestDemo\Assets\LuaScripts\dataConfig
)

pause