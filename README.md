# YakinikuMR
Hololens2と物体検出モデルを利用して、焼肉に新たなエンターテイメント性を持たせるアプリケーションです。

## HoloLens2+Unityの開発環境構築
以下のMRTKインストールガイドに従って環境構築を行ってください。  
see : https://docs.microsoft.com/ja-jp/windows/mixed-reality/mrtk-unity/?view=mrtkunity-2021-05

物体検出にはUnityの推論エンジンであるBarracudaを利用いるため、Package Managerからインストールが必要となります。  
see : https://docs.unity3d.com/Packages/com.unity.barracuda@1.0/manual/index.html

- - -

## この公開用リポジトリは以下の内容を含みます。
 - build 
Hololens2向けにビルド済みの焼肉インタラクションアプリケーションです。

 - hololens2_yakiniku_resources.unitypackage
アプリケーションに利用している推論用コード・ホログラムリソース・3D座標再構成用プログラム等を含むunityパッケージです。

 - SceneAssets
hololens2_yakiniku_resources.unitypackageに含まれる内容と同様のアセットが入ったフォルダです。
