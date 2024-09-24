@echo off
setlocal

dotnet build -c "Debug R14" -p:Platform="Any CPU"
dotnet build -c "Debug R15" -p:Platform="Any CPU"
dotnet build -c "Debug R16" -p:Platform="Any CPU"
dotnet build -c "Debug R17" -p:Platform="Any CPU"
dotnet build -c "Debug R18" -p:Platform="Any CPU"
dotnet build -c "Debug R19" -p:Platform="Any CPU"
dotnet build -c "Debug R20" -p:Platform="Any CPU"
dotnet build -c "Debug R21" -p:Platform="Any CPU"
dotnet build -c "Debug R22" -p:Platform="Any CPU"
dotnet build -c "Debug R23" -p:Platform="Any CPU"
dotnet build -c "Debug R24" -p:Platform="Any CPU"
dotnet build -c "Debug R25" -p:Platform="Any CPU"

zstd -f -19 -T0 -v "bin\Any CPU\Debug R14\net40\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR14.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Debug R15\net45\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR15.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Debug R16\net451\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR16.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Debug R17\net452\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR17.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Debug R18\net46\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR18.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Debug R19\net47\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR19.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Debug R20\net471\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR20.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Debug R21\net48\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR21.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Debug R22\net48\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR22.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Debug R23\net48\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR23.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Debug R24\net48\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR24.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Debug R25\net80\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR25.zstd"

endlocal
