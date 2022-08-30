namespace Memory_Eater
{
    internal class MB
    {
        private readonly int[] data;

        public MB(Random random)
        {
            data = new int[256 * 1024];
            random = new(DateTime.Now.Millisecond);
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = random.Next();
            }
        }
    }
}
