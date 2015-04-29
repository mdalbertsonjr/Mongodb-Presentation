$adminFile = "/users/vagrant/AdminDeployment.xml"
echo "Provioning DemoVM"

echo "Installing VisualStudio"
#choco install visualstudiocommunity2013
#echo "<?xml version="1.0" encoding="utf-8"?>
#<AdminDeploymentCustomizations xmlns="http://schemas.microsoft.com/wix/2011/AdminDeployment">
#   <BundleCustomizations TargetDir="default" NoWeb="default"/>
# 
#   <SelectableItemCustomizations>
#     <SelectableItemCustomization Id="Blend" Hidden="no" Selected="no" />
#     <SelectableItemCustomization Id="VC_MFC_Libraries" Hidden="no" Selected="yes" />
#     <SelectableItemCustomization Id="SQL" Hidden="no" Selected="no" />
#     <SelectableItemCustomization Id="WebTools" Hidden="no" Selected="no" />
#     <SelectableItemCustomization Id="SilverLight_Developer_Kit" Hidden="no" Selected="no" />
#     <SelectableItemCustomization Id="Win8SDK" Hidden="no" Selected="no" />
#     <SelectableItemCustomization Id="WindowsPhone80" Hidden="no" Selected="no" />
# 
#     <SelectableItemCustomization Id="BlissHidden" Selected="yes" />
#     <SelectableItemCustomization Id="HelpHidden" Selected="yes" />
#     <SelectableItemCustomization Id="LocalDBHidden" Selected="yes" />
#     <SelectableItemCustomization Id="NetFX4Hidden" Selected="yes" />
#     <SelectableItemCustomization Id="NetFX45Hidden" Selected="yes" />
#     <SelectableItemCustomization Id="PortableDTPHidden" Selected="yes" />
#     <SelectableItemCustomization Id="PreEmptiveDotfuscatorHidden" Selected="yes" />
#     <SelectableItemCustomization Id="PreEmptiveAnalyticsHidden" Selected="yes" />
#     <SelectableItemCustomization Id="ProfilerHidden" Selected="yes" />
#     <SelectableItemCustomization Id="ReportingHidden" Selected="yes" />
#     <SelectableItemCustomization Id="SDKTools3Hidden" Selected="yes" />
#     <SelectableItemCustomization Id="SDKTools4Hidden" Selected="yes" />
#     <SelectableItemCustomization Id="Silverlight5DRTHidden" Selected="yes" />
#     <SelectableItemCustomization Id="SQLCEHidden" Selected="yes" />
#     <SelectableItemCustomization Id="SQLCLRTypesHidden" Selected="yes" />
#     <SelectableItemCustomization Id="SQLDACHidden" Selected="yes" />
#     <SelectableItemCustomization Id="SQLDOMHidden" Selected="yes" />
#     <SelectableItemCustomization Id="SQLSharedManagementObjectsHidden" Selected="yes" />
#     <SelectableItemCustomization Id="TSQLHidden" Selected="yes" />
#     <SelectableItemCustomization Id="VCCompilerHidden" Selected="yes" />
#     <SelectableItemCustomization Id="VCCoreHidden" Selected="yes" />
#     <SelectableItemCustomization Id="VCDebugHidden" Selected="yes" />
#     <SelectableItemCustomization Id="VCDesigntimeHidden" Selected="yes" />
#     <SelectableItemCustomization Id="VCExtendedHidden" Selected="yes" />
#     <SelectableItemCustomization Id="WinJSHidden" Selected="yes" />
#     <SelectableItemCustomization Id="WinSDKHidden" Selected="yes" />
#  </SelectableItemCustomizations>
# 
#</AdminDeploymentCustomizations>" > $adminFile
If (-not (Test-Path '/users/vagrant/vs_community.exe')) {
(New-Object System.Net.WebClient).DownloadFile('http://download.microsoft.com/download/7/1/B/71BA74D8-B9A0-4E6C-9159-A8335D54437E/vs_community.exe', '/users/vagrant/vs_community.exe')
}

/users/vagrant/vs_community.exe /Passive /NoRestart /Log $env:temp\vs.log

#echo "Installing Chrome"
##choco install google-chrome-x64
#
#echo "Installing Git"
##choco install git
#
#echo "Cloning this repository"
##once this is up on github...
##or maybe make it public...
##git clone http://git.albertsonian.com/albertsonian/mongodb-presentation.git ~/mongodb-presentation
#
#echo "Downloading MongoDB"
##(New-Object System.Net.WebClient).DownloadFile('https://fastdl.mongodb.org/win32/mongodb-win32-x86_64-2008plus-ssl-3.0.2-signed.msi', '~\mongo.msi')
#
#echo "Installing MongoDB"
##msiexec /qn /i ~/mongo.msi
#
#echo "Downloading test database"
##(New-Object System.Net.WebClient).DownloadFile('https://google-drive', '~\spatial-db-dump')
