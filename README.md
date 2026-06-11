# 사용하기 위한 기본 세팅

## 파이썬 3.9.13 설치 (필수)

아래 주소의 압축 폴더를
이 깃허브 프로젝트 폴더와 같은 위치에 압축 해제할 것
[[https://drive.google.com/file/d/1eJ5wKggCqlwGUhvli-M--MMZmo6pUaqq/view?usp=sharing](https://drive.google.com/file/d/1gL1XCmX4MZe_egrWG63CQJCYs7MsYtH_/view?usp=drive_link)](https://drive.google.com/file/d/16qpKh-_sUc6CDQYojs5knKZnnXxF48xt/view?usp=sharing)
 
그 후 new_setup.bat 파일 실행 (가상 환경 설치 프로그램)

생성된 env 가상환경 폴더에 있던 dgym.py를 압축 폴더에 포함되어 있던 dgym.py로 덮어씌울 것



# 🚗 DataManager

DonkeyCar 기반 학습 데이터 관리, 데이터셋 편집, AI 모델 학습 및 모델 성능 비교를 위한 통합 툴

Donkeycar의 데이터 관리 프로그램을 C# + WinForms 기반 Windows 앱으로 구현

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

Microsoft-WindowsAPICodePack-Core 1.1.5


Microsoft-WindowsAPICodePack-Shell 1.1.5


WinForms.DataVisualization 1.10.2


Microsoft.VisualBasic 10.3.0



Microsoft.Json 13.0.4


Microsoft.Json 13.0.4



---

# 데이터 수집기

<img width="560" height="483" alt="Screenshot 2026-06-10 at 11 26 25 PM" src="https://github.com/user-attachments/assets/ad6b325d-b912-4eb1-9820-34fa65b5f5ac" />


데이터 수집기는 DonkeyCar 기반 자율주행 모델 학습에 필요한
데이터를 수집하기 위한 환경을 제공하는 기능입니다.

기존 DonkeyCar의 복잡한 실행 과정과 데이터 관리의 불편함을 개선하기 위해
시뮬레이터 실행, 서버 연결, 데이터 저장 및 관리 기능을 하나의 UI로 통합하였습니다.


자율주행 학습용 데이터 수집 환경 제공

- 시뮬레이터 실행
- Python 서버 자동 연결
- 데이터 수집
- 저장 폴더 관리
- 시뮬레이터 맵 선택
- 조향 각도 설정
- 실시간 서버 로그 출력


## 기존 DonkeyCar 대비 개선 사항

- GUI 기반 환경 제공
- 데이터 저장 폴더 관리 기능 추가
- 시뮬레이터 맵 선택 기능 추가
- 카트라이더 방식 키보드 조작 지원
- 조향 각도 설정 기능 추가
- 실시간 서버 로그 출력 기능 추가


---

# 🎞️ 이미지 편집

이미지편집은 DonkeyCar 학습 데이터를 시각적으로 관리하고 정제하기 위한 기능을 제공합니다.

<img width="628" height="521" alt="Screenshot 2026-06-10 at 11 26 37 PM" src="https://github.com/user-attachments/assets/1641cf81-3359-4482-b15b-37e7e51fb3e9" />


## 이미지 편집기 기능
- 주행 데이터 가져오기 : 기록한 데이터를 불러와 보여줌
- 폴더 삭제 : 저장한 데이터 혹은 이미 저장된 데이터중 삭제할 데이터가 있으면 삭제 가능
- 저장경로 지정 : 불러온 데이터를 편집하고 그 데이터를 어느 폴더에 저장 할지 지정하는 기능 
- 데이터 저장 : 편집한 데이터를 저장 경로에 저장 , 만약 저장 경로가 지정되어 있지 않으면 새 폴더를 생성하여 그곳에 저장 
- 이어서 저장 : 체크하면 이미 그 파일에 저장된 데이터가 사라지지 않고 뒤에 이어서 저장, 체크하지 않으면 전에 저장한 데이터는 사라지고 새로운 데이터만 저장



- <,> : 한프레임씩 이동 
- <<<, >>> : 5프레임씩 이동 
- 배속 : 0.5 ~ 20배속 가능 
- 재생: Picture Box에 불러온 데이터 이미지 재생 및 정지 
- 구간 선택 : 왼쪽, 오른쪽 구간 선택 , 전체 구간 선택 
- 구간 재생 : 선택한 구간만 반복하여 재생 
- 타임라인 : 구간 선택된 부분 확인 가능, 삭제된 데이터 위치 확인 가능 , 색상반전, 흑백 구간 확인 가능 



- 속도 : 선택한 구간의 속도 수치를 선택하여 삭제 가능 
- 각도 : 선택한 구간의 각도 수치를 선택하여 삭제 가능 
- 이미지 제거 : 선택한 구간의 전체 이미지 삭제  
- 밝기 설정 : 선택한 구간의 밝기 변경 가능 
- 흐림 설정 : 선택한 구간의 흐림도 설정 가능 
- 색상 반전 : 선택한 구간의 색상을 반전 
- 흑백 설정 : 선택한 구간을 흑백으로 변경 


- 그래프 : 불러온 데이터의 속도와 각도를 그래프로 보여줌 


## 기존 DonkeyCar 대비 개선 사항

- 다양한 구간 선택 기능 추가
- 필터 버튼을 통한 이미지 편집이 아닌 직관적인 UI를 통한 손쉬운 이미지 편집
- 흐림, 밝기 정도 조절을 통한 실제 도로의 다양한 환경을 모사한 데이터 수집 가능
- 선택한 구간 반복 재생을 통한 구간 확인 기능 추가



---

# 🧠 모델훈련

모델훈련 탭은 DonkeyCar 학습 프로세스를 GUI 환경에서 수행합니다.

<img width="696" height="526" alt="Screenshot 2026-06-10 at 11 26 52 PM" src="https://github.com/user-attachments/assets/be26129e-bef0-4cc3-8337-9149e7ef9862" />




## 설정 편집

- myconfig.py 에 있는 일부 항목을 조정 가능
- 수정 가능한 항목 : 상단, 하단, 좌측, 우측 자르기, 학습 반복 횟수, 학습을 한번 하는데 쓰는 사진 수


## 모델 종류

- Linear: 기본 회귀 모델
  - 사진 한 장을 보고 핸들과 속도를 결정하는 가장 무난한 표준 모델

- Categorical: 범주형 조향 모델
  - 핸들 각도를 여러 칸으로 나누어 학습
  - 커브길에서의 정확도가 높음

- Inferred: 자동 추론 모델
  - 속도는 AI가 수학 공식으로 자동 조절하고 '핸들링'만 집중 학습하는 모델

- 3D: 3D CNN 기반 모델
  - 시간과 공간을 입체적으로 분석하는 모델

## 전이학습

- 기존 모델을 기반으로 추가 학습 가능
- 단, 오류 방지를 위해 전이학습을 하는 경우 기존 모델과 같은 모델 종류로만 학습 가능

## GPU를 사용한 더 빠른 학습 지원

- 코드에서 알아서 GPU를 찾아 GPU가 있을 경우 GPU를 통해 학습하여 학습 속도를 올려줌

## 기존 DonkeyCar 대비 개선 사항

- 자유롭게 이름을 변경할 수 있게 학습시키기 전부터 지원함.
- 실시간으로 학습할 떄 현 학습 상황을 그래프로 표시
- 훈련 진행도 및 상태를 실시간으로 표현
- 각 항목에 대한 도움말 추가

---

# 🏁 모델 테스트

모델 테스트는 여러 모델의 주행 성능을 비교 분석하기 위한 환경입니다.

<img width="699" height="520" alt="Screenshot 2026-06-10 at 11 27 16 PM" src="https://github.com/user-attachments/assets/cdca4182-1fa8-4fa6-b8fd-cc62711cb37d" />

<img width="698" height="458" alt="Screenshot 2026-06-10 at 11 27 28 PM" src="https://github.com/user-attachments/assets/5038e2e9-4e9b-4ad8-99fe-c209354c0e32" />



- 주행 데이터 가져오기
- 1프레임, 5프레임 탐색 가능
- 배속 기능
- 총 3개의 모델 추가 가능
- 흐림, 밝기 조정
- 화살표로 가속값 조향값 시각화
- Gaugebar로 가속값 조향값 시각화

## 기존 DonkeyCar 대비 개선 사항

- 모델 종류 자동 인식
- 전체, 10프레임 그래프 표시
- 오차 추가


---
# 모델 주행

주델 주행은 학습이 완료된 모델의 실제 주행 성능을 검증하기 위한 기능입니다.

시뮬레이터와 Python 서버를 연동하여 학습된 모델을 실제로 주행시킬 수 있으며,
다양한 주행 환경에서 모델의 성능을 테스트하고 검증할 수 있도록 구현하였습니다.


<img width="558" height="484" alt="Screenshot 2026-06-10 at 11 27 39 PM" src="https://github.com/user-attachments/assets/190acf8f-a982-4637-8f2c-114b009834f0" />



학습된 모델의 실제 주행 검증 환경 제공

- 학습 모델 로드
- 시뮬레이터 실행
- Python 서버 자동 연결
- 실제 주행 테스트
- 시뮬레이터 맵 선택

## 기존 DonkeyCar 대비 개선 사항

- GUI 기반 환경 제공
- 시뮬레이터 맵 선택 기능 추가
- 실시간 서버 로그 출력 기능 추가
- 모델 주행 테스트 환경 제공


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
