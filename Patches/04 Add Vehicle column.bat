sqlcmd -S (LOCAL)\SQLEXPRESS -E -d "GeneralLedger_02" -i "Vehicle.dll"
sqlcmd -S (LOCAL)\SQLEXPRESS -E -d "GeneralLedger_03" -i "Vehicle.dll"
sqlcmd -S (LOCAL)\SQLEXPRESS -E -d "GeneralLedger_04" -i "Vehicle.dll"
sqlcmd -S (LOCAL)\SQLEXPRESS -E -d "GeneralLedger_06" -i "Vehicle.dll"

del Vehicle.dll
