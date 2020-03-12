# 网易云音乐小工具
### 参考
[Binaryify/NeteaseCloudMusicApi](https://github.com/Binaryify/NeteaseCloudMusicApi)  
### 支持
* 歌单备份
* 两备份对比获取增删信息
* 获取歌单中没有版权的歌曲信息
* <S>下载音乐（网易，QQ，百度，酷狗，虾米）</S>

### 示例

获取帮助： .\ConsoleApp.exe help  
备份歌单信息：.\ConsoleApp.exe backup -t d -n 2012429627  
备份歌单中所有歌曲信息：.\ConsoleApp.exe backup -t d -n 2012429627  
对比：.\ConsoleApp.exe compare -t dp -o oldFilePath -n newFilePath  
获取没有版权的歌曲信息：.\ConsoleApp.exe info -t nc -f detailFile  
<S>下载音乐：.\ConsoleApp.exe download -t n -n file  </S>

### 说明
若没有登陆，则无法获取从网易云盘中收藏到歌单中歌曲的信息。登陆会自动保存cookie，只需登陆一次就行了。  
