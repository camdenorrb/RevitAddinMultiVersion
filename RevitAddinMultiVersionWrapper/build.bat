@echo off
setlocal

rem Copy the files from the source to the destination
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR14.zst Resources\RevitAddinMultiVersionR14.zst
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR15.zst Resources\RevitAddinMultiVersionR15.zst
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR16.zst Resources\RevitAddinMultiVersionR16.zst
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR17.zst Resources\RevitAddinMultiVersionR17.zst
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR18.zst Resources\RevitAddinMultiVersionR18.zst
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR19.zst Resources\RevitAddinMultiVersionR19.zst
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR20.zst Resources\RevitAddinMultiVersionR20.zst
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR21.zst Resources\RevitAddinMultiVersionR21.zst
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR22.zst Resources\RevitAddinMultiVersionR22.zst
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR23.zst Resources\RevitAddinMultiVersionR23.zst
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR24.zst Resources\RevitAddinMultiVersionR24.zst
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR25.zst Resources\RevitAddinMultiVersionR25.zst

rem Build the project in Release mode
dotnet build -c "Release" -p:Platform="Any CPU"

endlocal
