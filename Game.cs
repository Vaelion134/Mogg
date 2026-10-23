namespace моггейм
{
    public class Game
    {
        public string Title { get; set; }
        public int Price { get; set; }

        public string PriceText
        {
            get { return Price == 0 ? "Бесплатно" : Price + " ₽"; }
        }
    }
}
