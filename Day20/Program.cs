using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    class ValueTriple
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }

        public ValueTriple(long x, long y, long z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static ValueTriple operator + (ValueTriple v1, ValueTriple v2)
        {
            return new ValueTriple(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
    }

    class Particle
    {
        public int ID { get; set; }
        public ValueTriple Position { get; set; }
        public ValueTriple Velocity { get; set; }
        public ValueTriple Acceleration { get; set; }

        public Particle(string initLine, int id)
        {
            // initLine in Format p=<XP,YP,ZP>, v=<XV,YV,ZV>, a=<XA,YA,ZA>
            string[] parameters = initLine.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string param in parameters)
            {
                char p = param[0];
                string[] values = param.Remove(param.Length - 1).Remove(0, 3).Split(',');
                switch(p)
                {
                    case 'p':
                        this.Position = new ValueTriple(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
                        break;
                    case 'v':
                        this.Velocity = new ValueTriple(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
                        break;
                    case 'a':
                        this.Acceleration = new ValueTriple(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
                        break;
                }
            }
            this.ID = id;
        }

        public void Move()
        {
            this.Velocity += this.Acceleration;
            this.Position += this.Velocity;
        }

        public long DistanceToOrigin()
        {
            return Math.Abs(this.Position.X) + Math.Abs(this.Position.Y) + Math.Abs(this.Position.Z);
        }

        public bool Match(Particle p)
        {
            return (this.Position.X == p.Position.X) && (this.Position.Y == p.Position.Y) && (this.Position.Z == p.Position.Z);
        }

        public bool Match(ValueTriple v)
        {
            return (this.Position.X == v.X) && (this.Position.Y == v.Y) && (this.Position.Z == v.Z);
        }

        public static ValueTriple CollisionDetection(Particle p1, Particle p2)
        {
            if (p1.Match(p2))
            {
                return new ValueTriple(p1.Position.X, p1.Position.Y, p1.Position.Z);
            }
            else
            {
                return null;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*StreamReader file = new StreamReader("input.txt");
            string line;

            List<Particle> particles = new List<Particle>();

            int count = 0;
            while ((line = file.ReadLine()) != null)
            {
                particles.Add(new Particle(line, count++));
            }

            file.Close();

            long minDistance;
            int particleClosestToOrigin = -1;

            for (int i = 0; i < 500; i++)
            //while(!Console.KeyAvailable)
            {
                minDistance = long.MaxValue;
                for (int p = 0; p < particles.Count; p++)
                {
                    particles[p].Move();
                    if (particles[p].DistanceToOrigin() < minDistance)
                    {
                        minDistance = particles[p].DistanceToOrigin();
                        particleClosestToOrigin = p;
                    }
                }
                ValueTriple collisionPosition = null;
                for (int p = 0; (p < particles.Count - 1) && (collisionPosition == null); p++)
                {
                    for (int p2 = p + 1; (p2 < particles.Count) && (collisionPosition == null); p2++)
                    {
                        collisionPosition = Particle.CollisionDetection(particles[p], particles[p2]);
                    }
                }
                if (collisionPosition != null)
                {
                    // remove colliders from list
                    //foreach(Particle part in particles)
                    for (int p = 0; p < particles.Count; p++)
                    {
                        if (particles[p].Match(collisionPosition))
                        {
                            particles.Remove(particles[p]);
                            p--;
                        }
                    }
                }
            }
            Console.WriteLine(string.Format("Particle closest to <0,0,0>: {0}", particles[particleClosestToOrigin].ID));*/
            Calculate2();
            Console.ReadLine();
        }

        public static string Calculate2()
        {
            Console.WriteLine("Day20 part 2");
            var lines = File.ReadAllLines("input.txt");
            List<Particle2> particles = new List<Particle2>();
            for (int x = 0; x < lines.Length; x++)
            {
                var line = lines[x];
                var parts = line.Split(new char[] { ',', '=', 'p', 'v', 'a', '<', '>', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(p => long.Parse(p)).ToList();

                particles.Add(new Particle2()
                {
                    Id = x,
                    XPos = parts[0],
                    YPos = parts[1],
                    ZPos = parts[2],
                    XVel = parts[3],
                    YVel = parts[4],
                    ZVel = parts[5],
                    XAccel = parts[6],
                    YAccel = parts[7],
                    ZAccel = parts[8]
                });
            }

            var minParticle = particles[0];
            int count = 0;
            while (true)
            {
                particles.ForEach(p => p.Tick());
                var collisions = particles.GroupBy(x => x.GetPosition()).Where(x => x.Count() > 1).ToDictionary(g => g.Key, g => g.ToList());
                if (collisions.Count() == 0)
                {
                    count++;
                    if (count > 500)
                    {
                        break;
                    }
                }
                foreach (var c in collisions)
                {
                    foreach (var bad in c.Value)
                    {
                        particles.Remove(bad);
                    }
                }
                var newMin = particles.OrderBy(p => p.GetDistance()).First();
                if (newMin != minParticle)
                {
                    minParticle = newMin;
                }
            }
            return particles.Count().ToString();
        }
    }

    public class Particle2
    {
        public int Id { get; set; }
        public long XPos { get; set; }
        public long YPos { get; set; }
        public long ZPos { get; set; }

        public long XVel { get; set; }
        public long YVel { get; set; }
        public long ZVel { get; set; }

        public long XAccel { get; set; }
        public long YAccel { get; set; }
        public long ZAccel { get; set; }

        public void Tick()
        {
            XVel += XAccel;
            YVel += YAccel;
            ZVel += ZAccel;

            XPos += XVel;
            YPos += YVel;
            ZPos += ZVel;
        }

        public long GetDistance()
        {
            return Math.Abs(XPos) + Math.Abs(YPos) + Math.Abs(ZPos);
        }

        public override bool Equals(object obj)
        {
            return (obj as Particle2).Id == this.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string GetPosition()
        {
            return string.Join(",", XPos, YPos, ZPos);
        }
    }
}
