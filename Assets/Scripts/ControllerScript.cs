using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour {

    private Coroutine cor;

    private Camera m_firstPersonCamera;
    [SerializeField]
    RotatePr _rotObj;

    [SerializeField] private GameObject _pistol;

    [SerializeField] private GameObject _prefabObject;

    private int j = 0;

    [SerializeField] private GameObject _startButton;

    [SerializeField] private GameObject _restartbutton;

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

    private bool isClicked = false;

    void Start () {
        //Input.gyro.enabled = true;
        m_firstPersonCamera = Camera.main;
       // StartGame();
	}
	
	void Update () {
        //gameObject.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, -Input.gyro.rotationRateUnbiased.z);
        if (BaseSDK.GetButton(1)&&isReadyToStart==false)
        {
            isReadyToStart = true;
            StartGame();
        }
        if (BaseSDK.GetButton(1)&&isReadyToStart&&isClicked==false)
        {
            _pistol.GetComponent<Animation>().Stop();
            _pistol.GetComponent<Animation>().Play("Shoot",PlayMode.StopAll);
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
                    Destroy(_enemy.gameObject,0.5f);
                    j--;
                    _score.text = (int.Parse(_score.text) + 1).ToString();
                }
            }
        }
	    if (!BaseSDK.GetButton(1))
	    {
	        isClicked = false;
	    }
	    if (BaseSDK.GetButton(2))
	    {
	        BaseSDK.ResetQuat();
            _rotObj.ResetOrient();//--------------MyEdition------------
           // _pricel.GetComponent<RotatePr>().ResetOrient();
        }
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
        // Vector3 randpos = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
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
        //_bullet.SetActive(true);
        //_pistol.SetActive(true);
        if (_startButton.activeSelf == true)
        {
            _startButton.SetActive(false);
        }
        _lifefill.gameObject.SetActive(true);
        _score.gameObject.SetActive(true);
        _score.text = "0";
        _pricel.SetActive(true);
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
        //_bullet.SetActive(false);
        _pricel.SetActive(false);
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
