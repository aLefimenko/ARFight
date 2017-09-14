using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour {

    private Coroutine cor;

    private Camera m_firstPersonCamera;

    [SerializeField] private GyroController1 _rotObj2;

    [SerializeField] private ControllerClipseCube _rotObj3;

    [SerializeField] private GameObject _prefabObject;

    private int j = 0;

    [SerializeField] private GameObject _startButton;

    [SerializeField] private GameObject _calibratebutton;

    private float _life = 1f;

    [SerializeField] private Image _lifefill;

    [SerializeField] private GameObject _positionForInst;

    private bool isReadyToStart = false;

    [SerializeField] private GameObject _bullet;

    [SerializeField] private GameObject _positionBullet;

    [SerializeField] private GameObject _pricel;

    [SerializeField] private GameObject _pricelMain;

    [SerializeField] private Text _score;

    [SerializeField] private GameObject _attackSprite;

    [SerializeField] private GameObject _prefabParticleDie;

    [SerializeField] private GameObject _prefabConnectToClipse;

    private Quaternion _quatForREset;

    private bool isClicked = false;

    private bool isReset = false;

    private bool isResetApp = false;

    private bool isReady = false;

    void Start () {
        Input.gyro.enabled = true;
        m_firstPersonCamera = Camera.main;
        _calibratebutton.SetActive(true);
	}
	
	void Update () {

        if ((BaseSDK.GetButton(1) || BaseSDK.GetButton(2)) &&isReadyToStart==false&&isReady)
        {
            isReadyToStart = true;
            StartGame();
        }

        if ((BaseSDK.GetButton(1) || BaseSDK.GetButton(2)) && isReadyToStart&&isClicked==false||Input.GetMouseButtonDown(0))
        {
            var bullet = Instantiate(_bullet, m_firstPersonCamera.transform.position, Quaternion.identity);
            bullet.SetActive(true);
            Destroy(bullet,0.5f);
            RaycastHit hit;
            isClicked = true;
            if (Physics.Raycast(m_firstPersonCamera.transform.position, _pricel.transform.forward, out hit, 1000f))
            {
                EnemyControll _enemy = hit.transform.GetComponent<EnemyControll>();
                if (_enemy != null)
                {
                    Destroy(Instantiate(_prefabParticleDie,_enemy.gameObject.transform.position,Quaternion.identity),1f);
                    Destroy(_enemy.gameObject);
                    j--;
                    _score.text = (int.Parse(_score.text) + 1).ToString();
                }
            }
        }

	    if (!BaseSDK.GetButton(1) || !BaseSDK.GetButton(2))
	    {
	        isClicked = false;
	    }

	    if (BaseSDK.GetButton(0) && !isResetApp)
	    {
            BaseSDK.ResetQuat();
            _prefabConnectToClipse.SetActive(true);
	        isResetApp = true;
            _calibratebutton.SetActive(false);
	        StartCoroutine(ResetOrinetaion());
	    }

	    if (!BaseSDK.GetButton(0) && isResetApp)
	    {
	        isResetApp = false;
	    }
    }

    IEnumerator ResetOrinetaion()
    {
        yield return new WaitForSeconds(4f);
        _rotObj2.ResetOrient();
        _prefabConnectToClipse.SetActive(false);
        _startButton.SetActive(true);
        isReady = true;
       
    }

    IEnumerator AttackSprite()
    {
        _attackSprite.SetActive(true);
        yield return  new WaitForSeconds(0.3f);
        _attackSprite.SetActive(false);
    }

    IEnumerator InstObjects()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        if (j < 7)
        {
            var horror = Instantiate(_prefabObject, _positionForInst.transform.position, Quaternion.identity);
            horror.transform.LookAt(m_firstPersonCamera.transform.position);
            horror.transform.rotation = Quaternion.Euler(0.0f, horror.transform.rotation.eulerAngles.y, horror.transform.rotation.z);
            j++;
        }
        Repeat();
    }

    void Repeat()
    {
        cor = StartCoroutine(InstObjects());
    }

    public void StartGame()
    {
        cor = StartCoroutine(InstObjects());
        if (_startButton.activeSelf)
        {
            _startButton.SetActive(false);
        }
        _lifefill.gameObject.SetActive(true);
        _score.gameObject.SetActive(true);
        _score.text = "0";
        _life = 1f;
        _lifefill.fillAmount = 1f;
    }

    public void StopGame()
    {
        var objectsHorror = GameObject.FindGameObjectsWithTag("enemy");
        isReadyToStart = false;
        foreach (GameObject go in objectsHorror)
        {
            Destroy(go);
        }
        _score.gameObject.SetActive(false);
        _startButton.SetActive(true);
        j = 0;
        StopCoroutine(cor);
    }

    public void Attack()
    {
        _life -= 0.2f;
        StartCoroutine(AttackSprite());
        if (_life > 0f)
        {
            _lifefill.fillAmount = _life;
        }
        else
        {
            StopGame();
        }
    }
}
