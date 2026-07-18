# ActionFit GameAnalytics SDK Bridge

GameAnalytics Unity SDK를 공식 OpenUPM source에서 정확한 버전으로 설치하기 위한 public source-only 브리지 패키지입니다. 이 패키지는 GameAnalytics 바이너리, archive, 인증 정보, game key, secret key 또는 vendor 설정 파일을 포함하지 않습니다.

## 설치

Unity Package Manager의 `Add package from git URL`에서 아래 URL을 사용하거나 `Packages/manifest.json`에 등록합니다.

```json
{
  "dependencies": {
    "com.actionfit.sdk.gameanalytics": "https://github.com/ActionFit-Editor/SDK_GameAnalytics.git#1.0.5"
  }
}
```

`ActionFit-Editor/SDK_GameAnalytics` repository와 `1.0.2` tag가 실제로 게시된 뒤 사용할 수 있습니다.

## 적용

1. `Tools > Package > Custom Package Manager > SDK Profiles`를 엽니다.
2. `Packages/com.actionfit.sdk.gameanalytics/Editor/SDKInstallProfile.json`을 선택합니다.
3. `Inspect (read-only)`로 기존 설치와 충돌을 확인합니다.
4. `Apply` 계획에서 dependency, scoped registry, ownership 변경을 검토합니다.
5. 표시된 plan을 별도 확인한 뒤 실행합니다.

profile은 다음 항목만 관리합니다.

- OpenUPM registry `https://package.openupm.com`
- scope `com.gameanalytics`와 `com.google.external-dependency-manager`
- registry package `com.gameanalytics.sdk@7.10.3`

이미 같은 dependency와 registry가 존재하면 호환 가능한 설치로 감지하고, 사용자가 소유권 인수를 선택하지 않는 한 destructive ownership을 가져가지 않습니다. 다른 버전, 다른 registry 또는 legacy `Assets/GameAnalytics` 설치가 감지되면 자동 덮어쓰기 대신 충돌로 중단합니다.

## 프로젝트 설정 보존

이 브리지는 GameAnalytics 설정, game key, secret key, scene/prefab, Addressables, Android Proguard 규칙과 프로젝트 analytics adapter를 생성·수정·삭제하지 않습니다. SDK 적용 전후에 기존 프로젝트 설정과 초기화 흐름을 별도로 검증해야 합니다.

## 공식 출처

- GameAnalytics Unity SDK `7.10.3`: https://github.com/GameAnalytics/GA-SDK-UNITY/releases/tag/7.10.3
- MIT License: https://github.com/GameAnalytics/GA-SDK-UNITY/blob/7.10.3/LICENSE.md
- Unity SDK documentation: https://docs.gameanalytics.com/event-tracking-and-integrations/sdks-and-collection-api/game-engine-sdks/unity/
- OpenUPM package: https://openupm.com/packages/com.gameanalytics.sdk/

## 메뉴

- `Tools > Package > GameAnalytics SDK Bridge > README`

SDK 설치·수정·제거는 Custom Package Manager의 plan 검토와 명시적 실행을 통해서만 수행합니다.

## 배포 경계

이 패키지를 프로젝트에 추가하는 것과 GitHub repository 생성, push, tag, catalog 등록, package publish는 별도 작업입니다.
