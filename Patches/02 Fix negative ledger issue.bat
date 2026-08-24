sqlcmd -S (LOCAL)\SQLEXPRESS -E -d GeneralLedger -Q "ALTER TABLE Sales ALTER COLUMN SaleDate DATETIME NULL"
sqlcmd -S (LOCAL)\SQLEXPRESS -E -d GeneralLedger -Q "ALTER TABLE Payment ALTER COLUMN PaymentDate DATETIME NULL"

sqlcmd -S (LOCAL)\SQLEXPRESS -E -d "GeneralLedger" -i "Sales.dll"
sqlcmd -S (LOCAL)\SQLEXPRESS -E -d "GeneralLedger" -i "Payment.dll"

del Sales.dll
del Payment.dll
