////////////////////////////////////////////////////////////////////////////////////////
///                                     Jepsian                                      ///
///                     (Have full control over Ichimoku cloud)                      ///
///                                                                                  ///
///         Publish date  18-DEC-2021                                                ///
///         Versian  1.0.0                                                           ///
///         By  Seyed Jafar Yaghoubi                                                 ///
///         Email  algo3xp3rt@gmail.com                                              ///
///         License  MIT                                                             ///
///                                                                                  ///
////////////////////////////////////////////////////////////////////////////////////////


using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.EasternStandardTime, AccessRights = AccessRights.None)]
    [Cloud("Span A", "Span B", Opacity = 0.2)]

    public class Jepsian : Indicator
    {

        #region User Input

        [Parameter("Tenkan sen", DefaultValue = 9, MinValue = 1, Group = "Period")]
        public int T_Period { get; set; }

        [Parameter("Kijun sen", DefaultValue = 26, MinValue = 1, Group = "Period")]
        public int K_Period { get; set; }

        [Parameter("Senkou span B", DefaultValue = 52, MinValue = 1, Group = "Period")]
        public int S_Period { get; set; }

        [Parameter("Quality Line", DefaultValue = 26, MinValue = 1, Group = "Period")]
        public int Q_Period { get; set; }

        [Parameter("Direction Line", DefaultValue = 26, MinValue = 1, Group = "Period")]
        public int D_Period { get; set; }

        [Parameter("Chikou Shift", DefaultValue = -26, MaxValue = -1, Group = "Shift")]
        public int C_Shift { get; set; }

        [Parameter("Kumo Shift", DefaultValue = 26, MinValue = 1, Group = "Shift")]
        public int K_Shift { get; set; }

        [Parameter("Quality Line", DefaultValue = 26, MinValue = 1, Group = "Shift")]
        public int Q_Shift { get; set; }

        [Parameter("Direction Line", DefaultValue = -26, MaxValue = -1, Group = "Shift")]
        public int D_Shift { get; set; }


        [Parameter("Show?", DefaultValue = false, Group = "Last Month H-L")]
        public bool MHL { get; set; }

        [Parameter("Length", DefaultValue = 26, MinValue = 1, Group = "Last Month H-L")]
        public int MHL_Length { get; set; }

        [Parameter("Color", DefaultValue = "Maroon", Group = "Last Month H-L")]
        public string M_Color { get; set; }

        [Parameter("Show?", DefaultValue = false, Group = "Last Day H-L")]
        public bool DHL { get; set; }

        [Parameter("Length", DefaultValue = 26, MinValue = 1, Group = "Last Day H-L")]
        public int DHL_Length { get; set; }

        [Parameter("Color", DefaultValue = "DD0070C0", Group = "Last Day H-L")]
        public string D_Color { get; set; }


        #endregion

        #region Japsian Output
        [Output("Tenkan sen", LineStyle = LineStyle.Solid, LineColor = "Red")]
        public IndicatorDataSeries T_Result { get; set; }

        [Output("Kijun sen", LineStyle = LineStyle.Solid, LineColor = "Blue")]
        public IndicatorDataSeries K_Result { get; set; }

        [Output("Chiku span", LineStyle = LineStyle.Solid, LineColor = "Green")]
        public IndicatorDataSeries C_Result { get; set; }

        [Output("Span A", LineStyle = LineStyle.Solid, LineColor = "Green")]
        public IndicatorDataSeries SA_Result { get; set; }

        [Output("Span B", LineStyle = LineStyle.Solid, LineColor = "FFFF6666")]
        public IndicatorDataSeries SB_Result { get; set; }

        [Output("Quality Line", LineStyle = LineStyle.Lines, LineColor = "Black")]
        public IndicatorDataSeries Q_Result { get; set; }

        [Output("Direction Line", LineStyle = LineStyle.Lines, LineColor = "Purple")]
        public IndicatorDataSeries D_Result { get; set; }

        #endregion

        #region Public Variables

        private IchimokuKinkoHyo _ICHI;
        private Bars Monthly, Daily;

        #endregion

        protected override void Initialize()
        {
            _ICHI = Indicators.IchimokuKinkoHyo(T_Period, K_Period, S_Period);
            if (MHL)
                Monthly = MarketData.GetBars(TimeFrame.Monthly);
            if (DHL)
                Daily = MarketData.GetBars(TimeFrame.Daily);
        }
        //END METHOD INITIALIZE

        public override void Calculate(int index)
        {

            T_Result[index] = _ICHI.TenkanSen.Last(0);
            K_Result[index] = _ICHI.KijunSen.Last(0);
            C_Result[index + C_Shift] = _ICHI.ChikouSpan.Last(0);
            SA_Result[index + K_Shift] = _ICHI.SenkouSpanA.Last(0);
            SB_Result[index + K_Shift] = _ICHI.SenkouSpanB.Last(0);
            Q_Result[index + Q_Shift] = _ICHI.KijunSen.Last(0);
            D_Result[index + D_Shift] = _ICHI.KijunSen.Last(0);

            if (MHL)
            {
                var m_high = Monthly.HighPrices.Last(1);
                var m_low = Monthly.LowPrices.Last(1);
                Chart.DrawTrendLine("Monthly High", Chart.LastVisibleBarIndex, m_high, Chart.LastVisibleBarIndex + MHL_Length, m_high, M_Color, 2, LineStyle.Solid);
                Chart.DrawTrendLine("Monthly Low", Chart.LastVisibleBarIndex, m_low, Chart.LastVisibleBarIndex + MHL_Length, m_low, M_Color, 2, LineStyle.Solid);
            }

            if (DHL)
            {
                var d_high = Daily.HighPrices.Last(1);
                var d_low = Daily.LowPrices.Last(1);
                Chart.DrawTrendLine("Daily High", Chart.LastVisibleBarIndex, d_high, Chart.LastVisibleBarIndex + DHL_Length, d_high, D_Color, 2, LineStyle.Solid);
                Chart.DrawTrendLine("Daily Low", Chart.LastVisibleBarIndex, d_low, Chart.LastVisibleBarIndex + DHL_Length, d_low, D_Color, 2, LineStyle.Solid);
            }

            Chart.DrawStaticText("Copyright", "algo3xp3rt@gmail.com", VerticalAlignment.Bottom, HorizontalAlignment.Left, Color.Gray);
        }
        //END METHOD CALCULATE
    }
    //END class DisplayDWMPipsonCharts
}
//END namespace cAlgo


