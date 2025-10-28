using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : SingleTon<BulletManager>
{
    List<BulletBase> _bulletPool = new List<BulletBase>();
    [SerializeField] GameObject _arrow;
    [SerializeField] GameObject _ball;
    [SerializeField] GameObject _stone;
    [SerializeField] GameObject _missile;

    public GameObject MakeBullet(BulletType type)//불릿 만드는 매서드.
    {
        //불릿 풀에서 타입 맞는 불릿 찾아서 활성화.
        foreach(BulletBase bullet in _bulletPool)
        {
            if(bullet._type == type && bullet.gameObject.activeSelf == false)
            {
                bullet.gameObject.SetActive(true);
                return bullet.gameObject;
            }
        }        
        //없다면 새로 생성해서 등록.
        {
            GameObject bullet = null;
            switch (type)
            {
                case BulletType.Arrow:
                    bullet = _arrow;
                    break;
                case BulletType.CannonBall:
                    bullet = _ball;
                    break;
                case BulletType.Boulder:
                    bullet= _stone;
                    break;
                case BulletType.Missile:
                    bullet=_missile;
                    break;
                default:
                    Debug.Log("알 수 없는 불릿 타입."+type);
                    return null;
            }
            BulletBase newBullet =  Instantiate(bullet).GetComponent<BulletBase>();
            newBullet._type = type;
            _bulletPool.Add(newBullet);
            return newBullet.gameObject;
        }        
    }
}
