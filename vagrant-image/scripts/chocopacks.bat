:: Ensure C:\Chocolatey\bin is on the path
set /p PATH=<C:\Windows\Temp\PATH
cmd /c choco feature enable -n allowGlobalConfirmation
:: Install all the things; for example:
:: cmd /c choco install 7zip
:: cmd /c choco install notepadplusplus
