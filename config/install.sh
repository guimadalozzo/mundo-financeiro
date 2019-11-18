# Register Microsoft key and feed
wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb 

# Install the .NET SDK
sudo apt-get update
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-sdk-3.0

# Clone project
git clone https://github.com/danielccunha/mundo-financeiro.git

# Set default user-secrets
cd mundo-financeiro/src/Api/MundoFinanceiro.Api/
dotnet user-secrets set ConnectionStrings:Default 'CONNECTION_STRING'
dotnet user-secrets set ApiSettings:CrawlerUrl 'CRAWLER_URL' # Already set crawler url

cd ../../Web/MundoFinanceiro.Web/
dotnet user-secrets set ConnectionStrings:Default 'CONNECTION_STRING'

cd ../../Data/MundoFinanceiro.Crawler/
dotnet user-secrets set ConnectionStrings:Default 'CONNECTION_STRING'

# Set replication url
cd ../MundoFinanceiro.Replicacao/
dotnet user-secrets set ConnectionStrings:Default 'REPLICATION_URL'

echo "Don't forget to update replications URL in the crawler"

# Restore and build project
cd ../..
dotnet restore
dotnet build
