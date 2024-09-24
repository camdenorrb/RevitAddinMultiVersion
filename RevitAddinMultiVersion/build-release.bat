@echo off
setlocal

dotnet build -c "Release R14" -p:Platform="Any CPU"
dotnet build -c "Release R15" -p:Platform="Any CPU"
dotnet build -c "Release R16" -p:Platform="Any CPU"
dotnet build -c "Release R17" -p:Platform="Any CPU"
dotnet build -c "Release R18" -p:Platform="Any CPU"
dotnet build -c "Release R19" -p:Platform="Any CPU"
dotnet build -c "Release R20" -p:Platform="Any CPU"
dotnet build -c "Release R21" -p:Platform="Any CPU"
dotnet build -c "Release R22" -p:Platform="Any CPU"
dotnet build -c "Release R23" -p:Platform="Any CPU"
dotnet build -c "Release R24" -p:Platform="Any CPU"
dotnet build -c "Release R25" -p:Platform="Any CPU"

zstd -f -19 -T0 -v "bin\Any CPU\Release R14\net40\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR14.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Release R15\net45\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR15.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Release R16\net451\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR16.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Release R17\net452\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR17.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Release R18\net46\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR18.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Release R19\net47\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR19.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Release R20\net471\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR20.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Release R21\net48\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR21.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Release R22\net48\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR22.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Release R23\net48\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR23.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Release R24\net48\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR24.zstd"
zstd -f -19 -T0 -v "bin\Any CPU\Release R25\net80\RevitAddinMultiVersion.dll" -o "bin\RevitAddinMultiVersionR25.zstd"

endlocal
