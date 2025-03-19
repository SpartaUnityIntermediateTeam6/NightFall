# 프로젝트명: NightFall
## 개요 
- 프로젝트 제작기간: 2025/03/13 ~ 2025/03/19
- 개발 엔진 및 언어: Unity, C#
- 멤버: 김상민(팀장), 순현빈, 정성욱, 김영환, 강기수

## 조작법
| 상호작용 | 상 (위) | 하 (아래) | 좌 (왼쪽) | 우 (오른쪽) | 인벤토리 | 건축 |
|:------:|:----:|:----:|:----:|:----:|:----:|:----:|
| 키 | W | S | A | D | I | F |



## 게임 설명
밤이 되면 적들이 비콘을 공격하러 몰려오고 플레이어는 적들에게서 비콘을 지켜야합니다.

클리어 조건: 일정 기간 동안 플레이어가 생존하면 해당 지역에서 탈출 가능한 비콘이 활성화 되고 비콘에 닿으면 클리어됩니다.

| 플레이어HP, 정신력, 비콘HP | 비콘 | 자원 |
|:-------:|:----:|:----:|
| <img src= https://github.com/user-attachments/assets/b6ed613b-da8c-4575-aa79-ce9d0c7a2752 width = "200" height = "100"> | <img src= https://github.com/user-attachments/assets/79aaa33a-bedd-44a5-9020-a956511395bd width = "150" height = "100"> | <img src= https://github.com/user-attachments/assets/6d064a46-7f2a-45eb-83ab-4803cf2e3f70 width = "150" height = "100"> |
| 플레이어 체력과 정신력, 비콘 체력을 UI로 표시 | 클리어 조건에 필요한 오브젝트 | 공격하여 자원 획득 가능 |


| Enemy | 건물(제작소) 건축 | 방어 건물 건축 | 타이머 |
|:-------:|:----:|:----:|:----:|
|<img src= https://github.com/user-attachments/assets/6407dfff-6899-4cad-b991-7057196d62ad width = "100" height = "150"> <img src= https://github.com/user-attachments/assets/28cc5772-55d4-4ffc-a883-4dc7f408ef6c width = "100" height = "150"> | <img src= https://github.com/user-attachments/assets/48930784-3d76-4726-a785-0aa5fe98b615 width = "100" height = "150"> <img src= https://github.com/user-attachments/assets/e3d03a5d-28cd-4f04-9f2b-6fe4f6e86e99 width = "100" height = "150"> | <img src= https://github.com/user-attachments/assets/5d8f6260-3c63-4036-804c-be25923a7e50 width = "100" height = "150"> <img src= https://github.com/user-attachments/assets/f3eca8aa-35bd-4280-8f4a-4865e47551d7 width = "100" height = "150">| ![image](https://github.com/user-attachments/assets/6b394899-d1d1-4800-81b6-e42af7448a24)>
|공격하여 자원 획득 가능제|작소에서 회복 아이템과 강화 아이템 제작 가능|Enemy 이동 방해 및 공격|낮 밤 구분을 위한 타이머 기능|
