助记词恢复辅助工具,仅支持BIP-39

第一步：在【有网络的电脑】上准备所有包文件
创建一个新的空文件夹，用来存放所有下载的包。例如，我们叫它 PackagePrep。

Bash

mkdir PackagePrep
cd PackagePrep
在这个文件夹里，创建一个临时的C#项目（我们只用它来下载包，用完就可以删掉）。

Bash

dotnet new console
添加 NBitcoin 包的引用到这个临时项目。

Bash

dotnet add package NBitcoin
【关键步骤】 运行 restore 命令，并将所有包（包括依赖）下载到一个指定的本地文件夹中。我们把这个文件夹命名为 all_packages。

Bash

dotnet restore --packages ./all_packages
执行完这条命令后，你会看到 PackagePrep 文件夹里多出了一个 all_packages 文件夹。打开它，你会发现里面不仅有 nbitcoin，还有 newtonsoft.json 等很多其他包。这正是我们需要的！

现在，将整个 all_packages 文件夹完整地复制到你的U盘里。

第二步：在【离线的电脑】上重新操作
回到你的 D:\Code\WalletFinder 项目文件夹。

删掉原来的 local_packages 文件夹，以免混淆。

将你从U盘拷贝过来的整个 all_packages 文件夹粘贴到 D:\Code\WalletFinder 目录下。

现在，打开终端，再次运行添加包的命令，但这次源文件夹是 all_packages：

PowerShell

dotnet add package NBitcoin --source ./all_packages
这一次，因为 all_packages 文件夹里包含了所有必需的依赖包，命令应该会成功执行，没有任何错误提示。

包成功安装后，你就可以编译和运行你的程序了：

PowerShell

dotnet run --configuration Release
