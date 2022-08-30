namespace Memory_Eater
{
    internal class DataChunk
    {
        private readonly List<MB> data;
        public DataChunk(int count)
        {
            Random random = new(DateTime.Now.Millisecond);
            data = new();
            for (int i = 0; i < count; ++i)
            {
                data.Add(new MB(random));
            }
        }
    }
}
