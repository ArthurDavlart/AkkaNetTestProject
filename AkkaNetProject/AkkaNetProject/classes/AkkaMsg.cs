namespace AkkaNetProject.classes
{
    public class AkkaMsg
    {
        public Element MinElement { get; set; }
        public Element NewElement { get; set; }

        public AkkaMsg(Element minElement, Element newElement)
        {
            MinElement = minElement;
            NewElement = newElement;
        }
    }
}
