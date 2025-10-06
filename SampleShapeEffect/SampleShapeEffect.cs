using System.ComponentModel.DataAnnotations;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Controls;
using YukkuriMovieMaker.Exo;
using YukkuriMovieMaker.Player.Video;
using YukkuriMovieMaker.Plugin.Effects;
using YukkuriMovieMaker.Plugin.Shape;
using YukkuriMovieMaker.Plugin.Tachie.Psd;
using YukkuriMovieMaker.Project.Items;
using YukkuriMovieMaker.Shape;

namespace SampleShapeEffect
{
    [VideoEffect("SampleShapeEffect", ["サンプル"], [], isAviUtlSupported: false)]
    internal class SampleShapeEffect : VideoEffectBase
    {
        public override string Label => "SampleShapeEffect";

        [Display(GroupName = "SampleShapeEffect", Name = "種類", Description = "図形の種類")]
        [EnumComboBox]
        public ShapeTypeEnum ShapeTypeEnum
        {
            get => shapeTypeEnum;
            set
            {
                if (GetShapeType(value) is Type type) ShapeType = type;
                Set(ref shapeTypeEnum, value);
            }
        }
        ShapeTypeEnum shapeTypeEnum = ShapeTypeEnum.四角形;

        public Type ShapeType { get => shapeType; set => Set(ref shapeType, value); }
        Type shapeType = typeof(QuadrilateralShapePlugin);

        private Type? oldShapeType;

        [Display(GroupName = "SampleShapeEffect", AutoGenerateField = true)]
        public IShapeParameter ShapeParameter { get => shapeParameter; set => Set(ref shapeParameter, value); }
        IShapeParameter shapeParameter = new RectangleShapeParameter(null);

        public override void BeginEdit()
        {
            oldShapeType = ShapeType;
            base.BeginEdit();
        }

        public override async ValueTask EndEditAsync()
        {
            if (ShapeParameter is null || oldShapeType != ShapeType)
            {
                ShapeParameter = ShapeFactory
                    .GetPlugin(ShapeType)
                    .CreateShapeParameter(ShapeParameter?.GetSharedData());
            }
            await base.EndEditAsync();
        }

        public override IEnumerable<string> CreateExoVideoFilters(int keyFrameIndex, ExoOutputDescription exoOutputDescription)
        {
            return [];
        }

        public override IVideoEffectProcessor CreateVideoEffect(IGraphicsDevicesAndContext devices)
        {
            return new SampleShapeEffectProcessor(devices, this);
        }

        protected override IEnumerable<IAnimatable> GetAnimatables() => [ShapeParameter];

        Type? GetShapeType(ShapeTypeEnum shapeType)
        {
            switch (shapeType)
            {
                case ShapeTypeEnum.背景:
                    return typeof(BackgroundShapePlugin);
                case ShapeTypeEnum.円:
                    return typeof(CircleShapePlugin);
                case ShapeTypeEnum.三角形:
                    return typeof(TriangleShapePlugin);
                case ShapeTypeEnum.四角形:
                    return typeof(QuadrilateralShapePlugin);
                case ShapeTypeEnum.五角形:
                    return typeof(PentagonShapePlugin);
                case ShapeTypeEnum.六角形:
                    return typeof(HexagonShapePlugin);
                case ShapeTypeEnum.星:
                    return typeof(StarShapePlugin);
                case ShapeTypeEnum.扇形:
                    return typeof(FanShapePlugin);
                case ShapeTypeEnum.くさび形:
                    return typeof(WedgeShapePlugin);
                case ShapeTypeEnum.矢印:
                    return typeof(ArrowShapePlugin);
                case ShapeTypeEnum.スーパー多角形:
                    return typeof(SuperformulaShapePlugin);
                case ShapeTypeEnum.集中線:
                    return typeof(ConcentrationLineShapePlugin);
                case ShapeTypeEnum.タイマー:
                    return typeof(TimerShapePlugin);
                case ShapeTypeEnum.波形:
                    return Types.AudioSpectrumShapePlugin;
                case ShapeTypeEnum.線:
                    return Types.LineShapePlugin;
                case ShapeTypeEnum.SVGファイル:
                    return typeof(SVGShapePlugin);
                case ShapeTypeEnum.手書きペン:
                    return Types.PenShapePlugin;
                case ShapeTypeEnum.数値:
                    return Types.NumberText;
                case ShapeTypeEnum.PSDファイル:
                    return typeof(PsdShapePlugin);
            }

            return null;
        }
    }
}
