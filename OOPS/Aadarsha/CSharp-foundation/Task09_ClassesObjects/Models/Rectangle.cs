namespace Task09_ClassesObjects.Models
{
    public class RectangleShape
    {
        public double? length;
        public double? breadth;
        public RectangleShape(double l, double b)
        {
            length = l;
            breadth = b;
        }
        public double CalculateArea()
        {
            return (double)(length * breadth);
        }
        public double CalculatePerimeter()
        {
            return (double)(2 * (length + breadth));
        }
    
    }

}