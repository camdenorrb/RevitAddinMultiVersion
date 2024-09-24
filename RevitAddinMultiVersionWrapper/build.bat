@echo off
setlocal

rem Copy the files from the source to the destination
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR14.zstd Resources\RevitAddinMultiVersionR14.zstd
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR15.zstd Resources\RevitAddinMultiVersionR15.zstd
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR16.zstd Resources\RevitAddinMultiVersionR16.zstd
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR17.zstd Resources\RevitAddinMultiVersionR17.zstd
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR18.zstd Resources\RevitAddinMultiVersionR18.zstd
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR19.zstd Resources\RevitAddinMultiVersionR19.zstd
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR20.zstd Resources\RevitAddinMultiVersionR20.zstd
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR21.zstd Resources\RevitAddinMultiVersionR21.zstd
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR22.zstd Resources\RevitAddinMultiVersionR22.zstd
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR23.zstd Resources\RevitAddinMultiVersionR23.zstd
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR24.zstd Resources\RevitAddinMultiVersionR24.zstd
copy /Y ..\RevitAddinMultiVersion\bin\RevitAddinMultiVersionR25.zstd Resources\RevitAddinMultiVersionR25.zstd

rem Build the project in Release mode
dotnet build -c "Release" -p:Platform="Any CPU"

endlocal
