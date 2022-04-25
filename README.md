# YakinikuMR
Hololens2と物体検出モデルを利用して、焼肉に新たなエンターテイメント性を持たせるアプリケーションです。

## HoloLens2+Unityの開発環境構築
以下のMRTKインストールガイドに従って環境構築を行ってください。  
see : https://docs.microsoft.com/ja-jp/windows/mixed-reality/mrtk-unity/?view=mrtkunity-2021-05

物体検出にはUnityの推論エンジンであるBarracudaを利用するため、こちらもPackage Managerからインストールが必要となります。  
see : https://docs.unity3d.com/Packages/com.unity.barracuda@1.0/manual/index.html

- - -

## この公開用リポジトリは以下の内容を含みます。
 - build  
Hololens2向けにビルド済みの焼肉インタラクションアプリケーションです。

 - hololens2_yakiniku_resources.unitypackage  
アプリケーションに利用している推論用コード・ホログラムリソース・3D座標再構成用プログラム等を含むunityパッケージです。

 - SceneAssets  
上記パッケージ(hololens2_yakiniku_resources.unitypackage)に含まれる内容と同様のアセットが入ったフォルダです。

- - -

## アプリケーション概要
このアプリケーションは焼肉の際に、肉の焼き時間を表示するホログラムタイマーを自動設置することで、新たなエンターテイメント性を生み出しつつ、肉の生焼けや焼きすぎを防ぐことができるアプリケーションです。  

![画像1](https://user-images.githubusercontent.com/104173409/165133948-c3b58bd4-4b90-492c-9858-62f32aca6dd4.jpg)  

## 🍖検出
アプリケーションで重要となる機能の一つが肉検出です。  
これはHoloLens2で撮影した画像から肉の矩形領域を検出し、その画像上の矩形領域や空間マッピングデータなどから、実空間上の肉の三次元座標を求める機能になります。  
本アプリケーションにおいて、この機能は空間マッピングとRayCastを利用し、視点を原点としてスクリーン座標系に変換した矩形領域の中心に向けたベクトルと、マッピング空間上のメッシュの交点を求めることで3次元座標を計算しています。
