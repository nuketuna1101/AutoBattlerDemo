AutoBattler Demo


오토 배틀러 데모 버전


플레이어 및 몬스터:
 - IState 인터페이스를 통해 행동 로직을 State 패턴으로 구현.
 - Player, Monster란 추상 클래스에 대한 세부 구현으로 직업별 플레이어 캐릭터, 몬스터 구현.

여러 가지 매니저: 싱글턴 패턴으로 선언된 매니저 클래스들.
 - 오브젝트 풀링 적용한 여러 PoolManager 클래스들.
 - 카메라, UI, 오디오 각각을 담당하는 매니저 클래스들.
 - 사용자 승패 관련 게임매니저
 - 전투 씬 전투 판정 관련 배틀매니저.
