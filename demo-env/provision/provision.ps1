#$adminFile = "/users/vagrant/AdminDeployment.xml"
echo "Provioning DemoVM"

echo "Installing VisualStudio"
#choco install visualstudiocommunity2013
cp /vagrant/provision/AdminDeployment.xml /users/vagrant/AdminDeployment.xml
If (-not (Test-Path '/users/vagrant/vs_community.exe')) {
(New-Object System.Net.WebClient).DownloadFile('http://download.microsoft.com/download/7/1/B/71BA74D8-B9A0-4E6C-9159-A8335D54437E/vs_community.exe', '/users/vagrant/vs_community.exe')
}

/users/vagrant/vs_community.exe /Passive /NoRestart /AdminFile /users/vagrant/AdminDeployment.xml  /Log $env:temp\vs.log
