using JigiJumper.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JigiJumper.UI
{
    public class HomeWindowUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _homeWindow = null;
        [SerializeField] private RectTransform _gameWindow = null;

        [Header("Home Window Buttons")]
        [SerializeField] private Button _btnStart = null;
        [SerializeField] private Button _btnRecords = null;
        [SerializeField] private Button _btnQuit = null;

        [Header("Game Mode Window Buttons")]
        [SerializeField] private Button _btnEasy = null;
        [SerializeField] private Button _btnNormal = null;
        [SerializeField] private Button _btnHard = null;
        [SerializeField] private Button _btnBack = null;

        List<Button> _gameButtons;
        List<Button> _homeButtons;

        private void Awake()
        {
            _homeWindow.gameObject.SetActive(false);
            _gameWindow.gameObject.SetActive(false);

            _gameButtons = new List<Button>();
            _homeButtons = new List<Button>();

            InitialComponents();

            GameManager.instance.jumper.OnPlanetReached += Jumper_OnPlanetReached;
        }

        private void Jumper_OnPlanetReached(JigiJumper.Actors.PlanetController arg1, JigiJumper.Actors.PlanetController arg2)
        {
            _homeWindow.gameObject.SetActive(true);
            _homeWindow.localScale = new Vector3(0, 0, 1);

            Utils.DoTweenUtility.DoShowWindow(_homeWindow,
                onComplete: () => ActiveButtons(true, _homeButtons)
            );
        }


        private void InitialComponents()
        {
            _btnEasy.onClick.AddListener(() => LoadTheGame(0));
            _btnNormal.onClick.AddListener(() => LoadTheGame(10));
            _btnHard.onClick.AddListener(() => LoadTheGame(20));
            _btnBack.onClick.AddListener(OnBtnBackClicked);

            _btnStart.onClick.AddListener(OnBtnStartClicked);
            _btnRecords.onClick.AddListener(OnBtnRecordClicked);
            _btnQuit.onClick.AddListener(OnBtnQuitClicked);

            _gameButtons.Add(_btnEasy);
            _gameButtons.Add(_btnNormal);
            _gameButtons.Add(_btnHard);
            _gameButtons.Add(_btnBack);

            _homeButtons.Add(_btnStart);
            _homeButtons.Add(_btnRecords);
            _homeButtons.Add(_btnQuit);

            ActiveButtons(false, _gameButtons);
            ActiveButtons(false, _homeButtons);
        }




        private void LoadTheGame(int startingLevel)
        {
            ActiveButtons(false, _gameButtons);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        private void OnBtnBackClicked()
        {
            TransitionToWindow(_gameWindow, _gameButtons, _homeWindow, _homeButtons);
        }

        private void OnBtnStartClicked()
        {
            TransitionToWindow(_homeWindow, _homeButtons, _gameWindow, _gameButtons);

            //ActiveButtons(false, _homeButtons);
            //Utils.DoTweenUtility.DoHideWindow(_homeWindow,
            //    onComplete: () =>
            //    {
            //        _homeWindow.localScale = new Vector3(0, 0, 1);
            //        _homeWindow.gameObject.SetActive(false);

            //        _gameWindow.gameObject.SetActive(true);
            //        _gameWindow.localScale = new Vector3(0, 0, 1);
            //        Utils.DoTweenUtility.DoShowWindow(_gameWindow,
            //            onComplete: () => ActiveButtons(true, _gameButtons));
            //    });
        }

        private void OnBtnRecordClicked()
        {
            //todo
        }

        private void OnBtnQuitClicked()
        {
            print("Quiting");
            Application.Quit();
        }

        private void ActiveButtons(bool active, IEnumerable<Button> buttons)
        {
            foreach (var button in buttons)
            {
                button.enabled = active;
            }
        }

        private void TransitionToWindow(
            RectTransform from,
            IEnumerable<Button> fromButtons,
            RectTransform to,
            IEnumerable<Button> toButtons)
        {

            ActiveButtons(false, fromButtons);
            Utils.DoTweenUtility.DoHideWindow(from,
                onComplete: () =>
                {
                    from.localScale = new Vector3(0, 0, 1);
                    from.gameObject.SetActive(false);

                    to.gameObject.SetActive(true);
                    to.localScale = new Vector3(0, 0, 1);
                    Utils.DoTweenUtility.DoShowWindow(to,
                        onComplete: () => ActiveButtons(true, toButtons));
                });

        }
    }
}