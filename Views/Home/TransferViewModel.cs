namespace ASP_SPD111.Views.Home
{

    /* Модель з даними, необхідними для відображення сторінки /Home/Transfer
     *
     */
    public record TransferViewModel
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }

        public string ControllerName { get; set; } = null;
    }
}
