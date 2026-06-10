# 사용하기 위한 기본 세팅

## 파이썬 3.9.13 설치 (필수)

아래 주소의 압축 폴더를
이 깃허브 프로젝트 폴더와 같은 위치에 압축 해제할 것
 
그 후 setup.bat 파일 실행 (가상 환경 설치 프로그램)

https://drive.google.com/file/d/1eJ5wKggCqlwGUhvli-M--MMZmo6pUaqq/view?usp=sharing



# 🚗 DataManager

> DonkeyCar 학습 데이터 관리, 데이터셋 편집, AI 모델 학습 및 모델 성능 비교를 위한 통합 Windows GUI 툴

DataManager는 DonkeyCar 기반 자율주행 프로젝트를 위한 올인원 관리 도구입니다.

기존 DonkeyCar 환경에서는 데이터 확인, 데이터셋 정제, 모델 학습, 성능 비교를 위해 여러 CLI 명령과 스크립트를 반복적으로 사용해야 했습니다.

DataManager는 이러한 작업을 하나의 직관적인 Windows GUI 환경으로 통합하여 데이터 수집부터 모델 검증까지의 전체 워크플로우를 효율적으로 관리할 수 있도록 설계되었습니다.

----

# 🖥️ 기술 스택

| 항목 | 내용 |
|--------|--------|
| Framework | .NET 10 |
| UI | WinForms |
| Language | C# |
| Python | env / Embeddable |
| Visualization | WinForms.DataVisualization |

---

# 📦 사용 패키지

xml Microsoft-WindowsAPICodePack-Core 1.1.5 
Microsoft-WindowsAPICodePack-Shell 1.1.5 
WinForms.DataVisualization 1.10.2 


---

# 🎞️ 이미지 편집

이미지편집은 DonkeyCar 학습 데이터를 시각적으로 관리하고 정제하기 위한 기능을 제공합니다.

## 지원 기능

### 데이터 로드

- Catalog 파일(.catalog) 파싱
- NDJSON 포맷 지원
- 이미지 자동 연결

### 데이터 탐색

- 프레임 단위 이동
- 이미지 미리보기
- Angle / Throttle 표시
- 썸네일 타임라인

### 재생 기능

- 재생 / 일시정지
- 구간 반복
- Playhead 이동
- 재생속도 조절
0.5x 1.0x 1.5x 2.0x 

### 이미지 필터: 흑백, 색상 반전, 밝기 조정, 박스 블러

### 데이터 정제: Angle 범위 삭제, Throttle 범위 삭제, 개별 프레임 삭제, 선택 프레임 숨김 처리

### 저장

#### 이어쓰기 저장: 기존 가공 데이터셋에 추가, 새 데이터셋 저장
---

# 🧠 모델훈련

모델훈련 탭은 DonkeyCar 학습 프로세스를 GUI 환경에서 수행합니다.

## 설정 편집

myconfig.py 항목을 직접 수정할 수 있습니다.


## 모델 종류

Linear: 기본 회귀 모델

Categorical: 범주형 조향 모델

Inferred: 자동 추론 모델

3D: 3D CNN 기반 모델


## 전이학습

기존 모델을 기반으로 추가 학습 가능





## GPU를 사용한 더 빠른 학습 지원




---

# 🏁 모델 테스트

모델 테스트는 여러 모델의 주행 성능을 비교 분석하기 위한 환경입니다.




## 모델 비교

최대 3개 모델 동시 비교 가능

각 모델별 예측 Angle, 예측 Throttle, 실제값, 오차 를 함께 표시합니다.



## 실시간 전처리

학습 환경을 재현하기 위해 밝기 조정, 블러 적용

---

# 📋 통합 로그 시스템

하단 이벤트 로그 테이블을 이용하여 메시지 박스를 줄인 편한 사용감

로그 색상 구분

| 종류 | 색상 |
|--------|--------|
| Error | 빨강 |
| Warning | 주황 |
| Info | 기본 |
|--------|--------|--------|

---


# License

Private Project
All Rights Reserve
