public interface IPoolable
{
    void OnGetFromPool();   // 풀에서 꺼낸 직후 (상태 리셋, 이펙트 재생 등)
    void OnReturnToPool();  // 풀에 반납 직전 (코루틴 정리, 애니 중지 등)
}