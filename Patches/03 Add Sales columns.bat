sqlcmd -S (LOCAL)\SQLEXPRESS -U InteractTech -P password123 -d "GeneralLedger_02" -i "Sales_02.dll"
sqlcmd -S (LOCAL)\SQLEXPRESS -U InteractTech -P password123 -d "GeneralLedger_03" -i "Sales_02.dll"
sqlcmd -S (LOCAL)\SQLEXPRESS -U InteractTech -P password123 -d "GeneralLedger_04" -i "Sales_02.dll"
sqlcmd -S (LOCAL)\SQLEXPRESS -U InteractTech -P password123 -d "GeneralLedger_06" -i "Sales_02.dll"

del Sales_02.dll
