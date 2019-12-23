using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Analytics;

namespace IceFoxStudio
{
    public class GamePlayManager : MonoBehaviour
    {
        [SerializeField] private int _startCoolTime = 600;
        private int _coolTime;

        private Transform Image_1;
        private Transform Image_2;
        private PictureFindManager pic1;
        private PictureFindManager pic2;
        private int amountPointDifferent;
        [SerializeField] private GamePlayUIManager uiManager;
        private int _countFindPoint;
        private bool _useHint;

        private int _currentChapter;
        private int _currentLevel;


        private void Awake()
        {
            _currentChapter = GameData.Singleton.CurrentChapterPlay;
            _currentLevel = GameData.Singleton.CurrentLevelPlay.Value;
            MessageBroker.Default.Receive<SelectWrongMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
            {
                if (_countFindPoint >= amountPointDifferent) return;

                Firebase.Analytics.FirebaseAnalytics.LogEvent("game_play_select_wrong_chapter" +
                                                              _currentChapter + "_lvl_" +
                                                              _currentLevel);

                _coolTime -= 10;
                ConvertTime(_coolTime);
                if (_coolTime <= 0)
                {
                    HandleGameOver();
                }

                SoundManager.singleton?.PlaySound("sfx_fail");
            });
            MessageBroker.Default.Receive<UseHintMessage>().TakeUntilDestroy(gameObject)
                .Subscribe(_ => { HandleUseHint(); });
        }

        private void Start()
        {
            LoadLevel();
            Observable.EveryUpdate().Where(l => isPause.Value == false).TakeUntilDestroy(gameObject)
                .Subscribe(_ => { HandleZoomInOut(); });
            HandleCoolTimeGamePlay();

            HandleShowFreeScopeHint();

            HandleCheckTutorial();
        }


        public void LoadLevel()
        {
            _useHint = false;
            _countFindPoint = 0;
            var path = "chapter" + _currentChapter + "/Level" + _currentLevel;
            var objectLvl = Resources.Load(path) as GameObject;
            uiManager.ClearLvl();
            var image1 = Instantiate(objectLvl, uiManager.mask_1);
            var image2 = Instantiate(objectLvl, uiManager.mask_2);

            Image_1 = image1.transform;
            Image_2 = image2.transform;

            pic1 = Image_1.GetComponent<PictureFindManager>();
            pic2 = Image_2.GetComponent<PictureFindManager>();

            amountPointDifferent = pic1.points.Length;

            pic1.SetPoint(true, HandleCbClick);
            pic2.SetPoint(false, HandleCbClick);
        }

        void HandleCheckTutorial()
        {
            if (TutorialManager.singleton.CompleteTutorialFindPointDifferent) return;
            pic1.points[0]
                .SetDifferentPointTutorial(() =>
                    {
                        MessageBroker.Default.Publish(new ShowHandTutorialMessage() {active = false});
                        TutorialManager.singleton.CompleteTutorialFindPointDifferent = true;
                    }
                );

            pic2.points[0]
                .SetDifferentPointTutorial(null);
            MessageBroker.Default.Publish(new ShowHandTutorialMessage()
                {active = true, pos = pic1.points[0].transform.position});
        }

        void HandleCbClick(string name, Vector3 pos)
        {
            var point = pic1.points.SingleOrDefault(p => !p.HasPickUp && p.gameObject.name == name);
            var point2 = pic2.points.SingleOrDefault(p => !p.HasPickUp && p.gameObject.name == name);


            if (point != null || point2 != null)
            {
                SoundManager.singleton.PlaySound("sfx_true");
                if (point2 != null)
                {
                    if (point2.isClue)
                    {
                        pic2.hasGetClue = true;
                    }
                }

                if (point != null)
                {
                    if (point.isClue)
                    {
                        pic1.hasGetClue = true;
                    }
                }

                HandleShowFreeScopeHint();
                _countFindPoint++;

                ChapterDataManager.singleton.chapterDatas[_currentChapter].levelDatas[_currentLevel].numberStar =
                    _countFindPoint;

                if (_countFindPoint >= amountPointDifferent)
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("game_play_select_correct_all_chapter" +
                                                                  _currentChapter + "_lvl_" +
                                                                  _currentLevel);
                    HandleLoadLevelLevel();
                }
            }

            if (_count == 0)
            {
                MessageBroker.Default.Publish(new ShowEffectSelectCorrectMessage() {startPos = pos});
                MessageBroker.Default.Publish(new ShowStarsMessage());
            }

            _count++;
            if (_count == 2)
            {
                _count = 0;
            }

            point2?.ClickPoint();
            point?.ClickPoint();
        }

        private void HandleLoadLevelLevel()
        {
            MessageBroker.Default.Publish(new PauseMessage() {pause = true});
            Observable.Timer(TimeSpan.FromSeconds(1)).TakeUntilDestroy(gameObject).Subscribe(_ =>
            {
                MessageBroker.Default.Publish(new PauseMessage() {pause = false});
                HandleWin();
            });
        }

        void HandleShowFreeScopeHint()
        {
            _disposableFreeScope?.Dispose();
            _disposableFreeScope = Observable.Interval(TimeSpan.FromSeconds(_durationFreeScope))
                .Where(l => _currentLevel < 7)
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ =>
                {
                    var point = pic2.points.FirstOrDefault(p => !p.HasPickUp);
                    if (point != null)
                        MessageBroker.Default.Publish(new FreeHintMessage() {pos = point.transform.position});
                });
        }

        private void HandleUseHint()
        {
            Debug.Log("HandleUseHint " + (Application.internetReachability == NetworkReachability.NotReachable));
            Debug.Log("HandleUseHint " + (AdsManager.singleton.IsReadyRewardVideo()));

            if (Application.internetReachability == NetworkReachability.NotReachable ||
                !AdsManager.singleton.IsReadyRewardVideo())
            {
                MessageBroker.Default.Publish(new ShowNotifyTxtMessage() { });
                return;
            }


            _useHint = true;
            var point = pic2.points.FirstOrDefault(p => !p.HasPickUp);

            if (point != null)
            {
                Image_1.localScale = Vector3.one;
                Image_2.localScale = Vector3.one;
                Image_1.localPosition = CalculatorBoundMoveImage(Image_1);
                Image_2.localPosition = CalculatorBoundMoveImage(Image_2);
                MessageBroker.Default.Publish(new ShowEffectHintMessage() {Pos = point.transform.position});
            }
        }

        private void HandleCoolTimeGamePlay()
        {
            _coolTime = _startCoolTime;
            Observable.Interval(TimeSpan.FromSeconds(1)).Where(l => _coolTime > 0).TakeUntilDestroy(gameObject)
                .Subscribe(_ =>
                {
                    _coolTime -= 1;
                    ConvertTime(_coolTime);
                    if (_coolTime <= 0)
                    {
                        HandleGameOver();
                    }
                });
        }

        private void HandleGameOver()
        {
            MessageBroker.Default.Publish(new ShowGameOverPopupMessage());
        }

        private void HandleWin()
        {
            isPause.Value = true;
            Observable.Timer(TimeSpan.FromSeconds(1)).TakeUntilDestroy(gameObject).Subscribe(_ =>
            {
                MessageBroker.Default.Publish(new ShowWinPopupMessage() {isShowVideo = _useHint});
            });
        }

        void ConvertTime(int _coolTime)
        {
            if (_coolTime < 0)
                _coolTime = 0;
            var time = TimeSpan.FromSeconds(_coolTime);
            uiManager.UpdateTime(time);
        }

        public BoolReactiveProperty isPause;
        public Vector2 firstPos;
        public Vector2 currentPos;
        public bool isMouseRight;
        public Vector2 firstPosRight;
        public Vector2 currentPosRight;

        public float firstDistanceMouseRightLeft;
        public float currentDistanceMouseRightLeft;

        public float widthImage = 1080;
        public float heighImage = 624;
        private Vector3 firstPosImage;
        private int _count;
        private IDisposable _disposableFreeScope;
        [SerializeField] private float _durationFreeScope = 20;


        public void HandleZoomInOut()
        {
            if (TutorialManager.singleton != null &&
                !TutorialManager.singleton.CompleteTutorialFindPointDifferent) return;

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(2))
            {
                isMouseRight = false;
                ExtensionMethod.isMoveFinger = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                isMouseRight = true;
                ExtensionMethod.isMoveFinger = true;
                currentPosRight = Input.mousePosition;
            }

            if (Input.GetMouseButton(1))
            {
                currentPosRight = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(1))
            {
            }

            if (isMouseRight)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    /* if (Image_1.localScale != Vector3.one)
                     {
                     }*/

                    firstPosImage = Image_1.localPosition;
                    firstPos = Input.mousePosition;
                    currentPos = Input.mousePosition;
                    firstDistanceMouseRightLeft = Vector2.Distance(currentPosRight, currentPos);
                }

                if (Input.GetMouseButton(0))
                {
                    currentPos = Input.mousePosition;
                    currentDistanceMouseRightLeft = Vector2.Distance(currentPosRight, currentPos);
                    var deltaDistace = currentDistanceMouseRightLeft - firstDistanceMouseRightLeft;
                    if (deltaDistace != 0)
                    {
                        Image_1.localScale += (Vector3) (Vector2.one * deltaDistace / 1000f);
                        Image_2.localScale += (Vector3) (Vector2.one * deltaDistace / 1000f);
                    }

                    var localScale = Image_1.localScale;
                    var localScale2 = Image_2.localScale;
                    Image_1.localScale = new Vector3(Mathf.Clamp(localScale.x, 1, 1.5f),
                        Mathf.Clamp(localScale.y, 1, 1.5f),
                        localScale.z);
                    Image_2.localScale = new Vector3(Mathf.Clamp(localScale2.x, 1, 1.5f),
                        Mathf.Clamp(localScale2.y, 1, 1.5f),
                        localScale2.z);

                    Image_1.localPosition = CalculatorBoundMoveImage(Image_1);
                    Image_2.localPosition = CalculatorBoundMoveImage(Image_2);
                    firstDistanceMouseRightLeft = currentDistanceMouseRightLeft;
                }

                if (Input.GetMouseButtonUp(0))
                {
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    /* if (Image_1.localScale != Vector3.one)
                     {
                     }*/

                    firstPos = Input.mousePosition;
                    currentPos = Input.mousePosition;
                }

                if (Input.GetMouseButton(0) && Image_1.localScale != Vector3.one && Image_2.localScale != Vector3.one)
                {
                    currentPos = Input.mousePosition;
                    var deltaPos = currentPos - firstPos;
                    if (deltaPos != Vector2.zero)
                    {
                        Image_1.localPosition += (Vector3) deltaPos;
                        Image_2.localPosition += (Vector3) deltaPos;
                        ExtensionMethod.isMoveFinger = true;
                    }

                    Image_1.localPosition = CalculatorBoundMoveImage(Image_1);
                    Image_2.localPosition = CalculatorBoundMoveImage(Image_2);
                    firstPos = currentPos;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    // Image_1.localPosition = firstPosImage;
                    ExtensionMethod.isMoveFinger = false;
                }
            }
#else
   var touches = Input.touches;

        if (touches.Length == 1)
        {
            if (touches[0].phase == TouchPhase.Began)
            {
            }

            if (touches[0].phase == TouchPhase.Moved)
            {
                ExtensionMethod.isMoveFinger = true;
                Image_1.localPosition += (Vector3) touches[0].deltaPosition;
                Image_2.localPosition += (Vector3) touches[0].deltaPosition;

                Image_1.localPosition = CalculatorBoundMoveImage(Image_1);
                Image_2.localPosition = CalculatorBoundMoveImage(Image_2);
            }

            if (touches[0].phase == TouchPhase.Ended)
            {
                ExtensionMethod.isMoveFinger = false;
            }
        }

        if (touches.Length == 2)
        {
            Vector2 posTouch1 = touches[0].position;
            Vector2 posTouch2 = touches[1].position;
            if (touches[0].phase == TouchPhase.Began)
            {
                posTouch1 = touches[0].position;
            }

            if (touches[1].phase == TouchPhase.Began)
            {
                posTouch2 = touches[1].position;
                firstDistanceMouseRightLeft = Vector2.Distance(posTouch1, posTouch2);
            }

            if (touches[0].phase == TouchPhase.Moved)
            {
                ExtensionMethod.isMoveFinger = true;
                posTouch1 = touches[0].position;
            }

            if (touches[1].phase == TouchPhase.Moved)
            {

                ExtensionMethod.isMoveFinger = true;
                posTouch2 = touches[1].position;
                currentDistanceMouseRightLeft = Vector2.Distance(posTouch1, posTouch2);
            }

            var deltaDistace = currentDistanceMouseRightLeft - firstDistanceMouseRightLeft;
            if (deltaDistace != 0 && firstDistanceMouseRightLeft != 0 && currentDistanceMouseRightLeft != 0)
            {
                Image_1.localScale += (Vector3) (Vector2.one * deltaDistace / 1000f);
                Image_2.localScale += (Vector3) (Vector2.one * deltaDistace / 1000f);
            }

            var localScale = Image_1.localScale;
            var localScale2 = Image_2.localScale;
            Image_1.localScale = new Vector3(Mathf.Clamp(localScale.x, 1, 1.5f),
                Mathf.Clamp(localScale.y, 1, 1.5f),
                localScale.z);
            Image_2.localScale = new Vector3(Mathf.Clamp(localScale2.x, 1, 1.5f),
                Mathf.Clamp(localScale2.y, 1, 1.5f),
                localScale2.z);

            Image_1.localPosition = CalculatorBoundMoveImage(Image_1);
            Image_2.localPosition = CalculatorBoundMoveImage(Image_2);
            firstDistanceMouseRightLeft = currentDistanceMouseRightLeft;

            if (touches[0].phase == TouchPhase.Ended)
            {
            }
            if (touches[1].phase == TouchPhase.Ended)
            {
            }
        }

        if (touches.Length == 0)
        {
            firstDistanceMouseRightLeft = 0;
            currentDistanceMouseRightLeft = 0;
        }
#endif
        }


        Vector3 CalculatorBoundMoveImage(Transform image)
        {
            var currentLocalScale = image.localScale;
            var maxX = (currentLocalScale.x - 1) * widthImage / 2f;
            var maxY = (currentLocalScale.y - 1) * heighImage / 2f;
            var newX = Mathf.Clamp(image.localPosition.x, -maxX, maxX);
            var newY = Mathf.Clamp(image.localPosition.y, -maxY, maxY);
            return new Vector3(newX, newY, image.localPosition.z);
        }
    }

    internal class ShowStarsMessage
    {
    }

    internal class UseHintMessage
    {
    }

    internal class ShowGameOverPopupMessage
    {
    }
}