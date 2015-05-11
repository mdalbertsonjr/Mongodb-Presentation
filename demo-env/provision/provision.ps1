echo "Downloading MongoDB"
(New-Object System.Net.WebClient).DownloadFile('https://fastdl.mongodb.org/win32/mongodb-win32-x86_64-2008plus-ssl-3.0.2-signed.msi', '/users/vagrant/mongo.msi')
