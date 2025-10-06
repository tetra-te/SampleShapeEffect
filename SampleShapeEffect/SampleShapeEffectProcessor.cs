using Vortice.Direct2D1;
using Vortice.Direct2D1.Effects;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Player.Video;
using YukkuriMovieMaker.Plugin.Shape;

namespace SampleShapeEffect
{
    internal class SampleShapeEffectProcessor : IVideoEffectProcessor
    {
        readonly SampleShapeEffect item;
        readonly IGraphicsDevicesAndContext devices;

        DisposeCollector disposer = new();

        bool isFirst = true;
        IShapeParameter? shapeParameter;

        IShapeSource? shapeSource;

        AffineTransform2D wrap;

        public ID2D1Image Output { get; }

        public SampleShapeEffectProcessor(IGraphicsDevicesAndContext devices, SampleShapeEffect item)
        {
            this.item = item;
            this.devices = devices;

            wrap = new AffineTransform2D(devices.DeviceContext);
            disposer.Collect(wrap);

            Output = wrap.Output;
            disposer.Collect(Output);
        }

        public DrawDescription Update(EffectDescription effectDescription)
        {
            var shapeParameter = item.ShapeParameter;

            if (isFirst || this.shapeParameter != shapeParameter)
            {
                if (shapeSource is not null)
                {
                    disposer.RemoveAndDispose(ref shapeSource);
                }
                shapeSource = shapeParameter.CreateShapeSource(devices);

                disposer.Collect(shapeSource);

                isFirst = false;
                this.shapeParameter = shapeParameter;
            }

            if (shapeSource is not null)
            {
                shapeSource.Update(effectDescription);

                wrap.SetInput(0, shapeSource.Output, true);              
            }
            return effectDescription.DrawDescription;
        }

        public void ClearInput()
        {
            wrap.SetInput(0, null, true);
        }

        public void Dispose()
        {
            disposer.Dispose();
        }

        public void SetInput(ID2D1Image? input)
        {
        }       
    }
}