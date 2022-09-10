namespace Memory_Eater
{
    internal class MB
    {
        private readonly int[] _data;

        public MB(Random random)
        {
            _data = new int[256 * 1024];
            random = new(DateTime.Now.Millisecond);
            for (int i = 0; i < _data.Length; ++i)
            {
                _data[i] = random.Next();
            }
        }
    }
}
