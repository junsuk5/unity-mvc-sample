# Architecture Demo

이 프로젝트는 Unity의 **Feature-Based Architecture**를 적용한 데모 애플리케이션입니다. 모듈화된 기능 단위(Feature)로 코드를 구성하여 유지보수성과 확장성을 높이는 데 중점을 두고 있습니다.

## 🚀 시작하기

프로젝트를 실행하기 위해 다음 단계를 따라주세요.

### 사전 요구 사항

- **Unity 버전**: `6000.0.34f1` 이상

### 🛠️ Firebase 설정 가이드

이 프로젝트는 Firebase와 연동이 필요합니다. 아래 설정 과정을 순서대로 진행해주세요.

1.  **Firebase SDK 설치**
    - [Firebase Unity SDK](https://firebase.google.com/docs/unity/setup) 공식 페이지에서 SDK를 다운로드합니다.
    - 다운로드한 SDK에서 다음 두 가지 패키지를 Unity 프로젝트로 임포트(Import)합니다.
        - `FirebaseAnalytics.unitypackage`
        - `FirebaseFirestore.unitypackage`

2.  **패키지 이름 변경**
    - Unity 에디터 상단 메뉴에서 `File > Build Settings...`를 엽니다.
    - `Player Settings...` 버튼을 클릭합니다.
    - `Player > Other Settings > Identification` 섹션으로 이동합니다.
    - **Package Name**을 `com.company.productname` 형식에 맞춰 **본인의 고유한 패키지 이름으로 수정**합니다.

3.  **Firebase 콘솔에 프로젝트 추가**
    - [Firebase 콘솔](https://console.firebase.google.com/)로 이동하여 새 프로젝트를 생성하거나 기존 프로젝트를 선택합니다.
    - 프로젝트 개요에서 **Unity 아이콘**을 클릭하여 새 앱을 추가합니다.
    - **앱 등록** 과정에서 방금 전 Unity에서 설정한 **동일한 패키지 이름**을 입력합니다.

4.  **구성 파일 추가**
    - Firebase 콘솔에서 `google-services.json` 파일을 다운로드합니다.
    - 다운로드한 `google-services.json` 파일을 Unity 프로젝트의 **`Assets/`** 폴더 바로 아래에 드래그 앤 드롭으로 추가합니다.

## ▶️ 프로젝트 실행

위의 모든 설정이 완료되면 Unity 에디터에서 프로젝트를 열고 **Play 버튼**을 눌러 정상적으로 실행할 수 있습니다.

---

## 🏛️ 아키텍처 개요

- **Feature-Based Architecture**: 각 기능(Feature)이 독립적인 모듈로 구성되어 있습니다. (`Assets/Scripts/Feature/*`)
- **공통 모듈**: 여러 기능에서 공유하는 코드는 `Common` 디렉토리에 있습니다. (`Assets/Scripts/Common/*`)
- **비동기 처리**: `UniTask`를 사용하여 현대적이고 효율적인 비동기 코드를 작성합니다.
- **에셋 관리**: `Addressable Assets` 시스템을 통해 에셋을 관리하여 유연성과 성능을 확보합니다.

## ⚙️ 주요 기술 스택

- **Asynchronous**: `UniTask`
- **Asset Management**: `Addressable Assets`
- **Input System**: `Input System`
- **Rendering**: `Universal Render Pipeline (URP)`
