using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    List<Bullet> _bulletPool;
    [SerializeField] GameObject _arrow;
    [SerializeField] GameObject _ball;
    [SerializeField] GameObject _stone;
    [SerializeField] GameObject _missile;

    private void Awake()//불릿풀 뉴 할당.
    {
        _bulletPool = new List<Bullet>();
    }

    public void MakeBullet(BulletType type)//불릿 만드는 매서드.
    {
        bool isAlive = false;

        for (int i = 0; i < _bulletPool.Count; i++) //타입 맞는 불릿 존재하면 활성화.
        {
            if (_bulletPool[i]._type == type && _bulletPool[i].gameObject.activeSelf == false)
            {
                _bulletPool[i].gameObject.SetActive(true);
                isAlive = true;
                break;
            }
        }
        if (isAlive == false) //없다면 새로 생성해서 등록.
        {
            switch (type)
            {
                case BulletType.Arrow:
                    _bulletPool.Add(Instantiate(_arrow).GetComponent<Bullet>());
                    break;
                case BulletType.Ball:                    
                    _bulletPool.Add(Instantiate(_ball).GetComponent<Bullet>());
                    break;
                case BulletType.Stone:                    
                    _bulletPool.Add(Instantiate(_stone).GetComponent<Bullet>());
                    break;
                case BulletType.Missile:                    
                    _bulletPool.Add(Instantiate(_missile).GetComponent<Bullet>());
                    break;
            }
        }
    }    
}
