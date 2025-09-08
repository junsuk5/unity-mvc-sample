# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

프로젝트 개요
- 엔진: Unity 6000.0.34f1 (Unity 6)
- 주요 패키지: UniTask, Addressables, TextMeshPro, Input System, URP, NUnit(Test Framework)
- 어셈블리: Scripts.asmdef(프로덕션 코드), EditMode.Tests.asmdef(에디트모드 테스트)

자주 쓰는 명령어 (macOS, zsh)
- 사전 준비: Unity CLI 경로를 환경 변수로 지정하세요.

```bash path=null start=null
export UNITY_PATH="/Applications/Unity/Hub/Editor/6000.0.34f1/Unity.app/Contents/MacOS/Unity"
```

- 프로젝트 열기 (Hub 또는 직접 실행)
```bash path=null start=null
# Unity Hub로 열기 (프로젝트 루트에서 실행)
open -a "Unity Hub" "$PWD"

# 에디터 직접 실행
"$UNITY_PATH" -projectPath "$PWD"
```

- 테스트 실행 (EditMode 전부)
```bash path=null start=null
mkdir -p artifacts
"$UNITY_PATH" \
  -batchmode -quit \
  -projectPath "$PWD" \
  -runTests -testPlatform EditMode \
  -assemblyNames "EditMode.Tests" \
  -testResults "artifacts/test-results-editmode.xml" \
  -logFile -
```

- 단일 테스트 실행 (메서드명 또는 FQN 필터)
```bash path=null start=null
# 방법 1) 메서드명 부분 일치로 필터
"$UNITY_PATH" -batchmode -quit -projectPath "$PWD" -runTests -testPlatform EditMode \
  -assemblyNames "EditMode.Tests" \
  -testFilter "ImageRepository_동작_확인" -logFile -

# 방법 2) (필요시) 정규화된 이름으로 필터
"$UNITY_PATH" -batchmode -quit -projectPath "$PWD" -runTests -testPlatform EditMode \
  -assemblyNames "EditMode.Tests" \
  -testFilter "Tests.EditMode.ImageRepositoryTests.ImageRepositoryTest.ImageRepository_동작_확인" -logFile -
```

- 린트/포맷
  - 별도의 CLI 린터/포맷터는 설정되어 있지 않습니다.
  - 스타일 가이드는 .cursor/rules/csharp-coding-standards.mdc를 따르세요.

- 빌드
  - 스크립트형 CLI 빌드 메서드(예: Build.BuildPlayer)는 현재 저장소에 존재하지 않습니다.
  - 에디터 메뉴(File > Build Settings)로 빌드하세요. 씬 포함 여부를 확인하십시오.

상위 수준 아키텍처
- 모듈
  - Common: 이벤트 시스템(Common.EventSystem), 상태 머신(Common.StatementSystem), 라우트(Common.Routes)
  - Data: DataSource(원격/Pixabay, Mock), Repository(ImageRepository), Dto/Mapper(JSON→DTO 변환)
  - Feature: Home, Play, Search, SearchDetail 각각 Controller(로직)와 View(UI)로 분리

- 이벤트 주도 흐름 (예: Search)
  1) View(SearchCanvas)가 입력을 받아 OnClickSearchButton 이벤트를 발행(Emit)
  2) Controller(SearchController)가 이벤트를 수신(IMonoEventListener)
  3) Repository(ImageRepository)가 DataSource(Mock 또는 원격) 통해 데이터 취득
  4) Mapper(Json→ImageDto) 후 View(ImageGridView)에 표시

- 씬/라우팅
  - Common.Routes.RouteNames에서 씬 경로를 상수로 관리: Home, Play(Assets/Scripts/Feature/Play/Play.unity), Search(Assets/Scripts/Feature/Search/Search.unity)
  - 전환은 SceneManager.LoadScene(...) 사용 (Addressables 예시는 주석 처리됨)

- 비동기/상태 관리
  - UniTask 기반의 StateMachine(Common.StatementSystem.StateMachine)으로 비동기 상태 전환(IState<T>.Enter/Exit)
  - Common.EventSystem은 상위 계층으로 이벤트를 전파하며 EventChain(Break/Continue)로 전파 제어

- 어셈블리/의존성
  - Scripts.asmdef: Unity.Addressables, UniTask, Unity.TextMeshPro 참조
  - EditMode.Tests.asmdef: Scripts, UniTask, Unity TestRunner, NUnit 참조(에디터 전용)

중요 규칙 하이라이트 (.cursor/rules/)
- 커밋 메시지 (.cursor/rules/commit-messages.mdc)
  - 컨벤셔널 커밋 + Unity 특화 타입 지원(asset/scene/prefab/shader/config)
  - 예: feat(play): 플레이어 이동 시스템 추가

- C# 코딩 표준 (.cursor/rules/csharp-coding-standards.mdc)
  - 네이밍: 클래스/공용 메서드/프로퍼티 PascalCase, 인터페이스 I 접두, 비공개 필드 camelCase, 상수 UPPER_SNAKE_CASE
  - MonoBehaviour 수명주기 메서드 활용, UniTask 선호(코루틴보다)
  - 이벤트 시스템 패턴(IMonoEventDispatcher/Listener)과 상태 머신(IState<T>) 사용 예시 포함

- Feature 개발 가이드 (.cursor/rules/feature-development.mdc)
  - Feature/[Name]/Controller, View, 씬을 한 모듈로 관리
  - 이벤트 기반 통신, 필요한 경우 Addressables로 씬 로딩
  - 신규 Feature 생성 시 Controller/View 템플릿과 체크리스트 제공

문서 참고
- 상태 머신 가이드: Assets/Scripts/Common/StatementSystem/README.md

운영 팁
- 테스트 아티팩트는 artifacts/ 하위에 저장되며 CI에서 수집하기 용이합니다.
- 씬을 추가할 때는 RouteNames 상수와 Build Settings의 씬 목록을 함께 유지하세요.

