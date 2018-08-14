using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Windows;
using System.Drawing;
using SharpDX.DXGI;
using D3D11 = SharpDX.Direct3D11;
using SharpDX.Direct3D;
using SharpDX.Mathematics;

namespace Graphics
{
	public class Visualiser : IDisposable
	{
		//Main entry
		private RenderForm renderForm;

		// Window parameters
		public const int Width = 1280;
		public const int Height = 720;

		//for swap chain
		private D3D11.Device d3dDevice;
		private D3D11.DeviceContext d3dDeviceContext;
		private SwapChain swapChain;
		private D3D11.RenderTargetView renderTargetView;

		//initializing window
		public Visualiser()
		{
			renderForm = new RenderForm("Ray tracing");
			renderForm.ClientSize = new Size(Width, Height);
			renderForm.AllowUserResizing = false;

			InitializeDeviceResources();
		}

		//entry point
		public void Run()
		{
			RenderLoop.Run(renderForm, RenderCallback);
		}

		//startup settings
		private void InitializeDeviceResources()
		{
			//params,FPS,buffer pixel format
			ModeDescription backBufferDesc = new ModeDescription(Width, Height, new Rational(30, 1), Format.R8G8B8A8_UNorm);

			SwapChainDescription swapChainDesc = new SwapChainDescription()
			{
				ModeDescription = backBufferDesc,
				SampleDescription = new SampleDescription(1, 0),
				Usage = Usage.RenderTargetOutput,
				BufferCount = 1,
				OutputHandle = renderForm.Handle,
				IsWindowed = true
			};

			//GPU,Special flag,descriptor for swap chain,holders 
			D3D11.Device.CreateWithSwapChain(DriverType.Hardware, D3D11.DeviceCreationFlags.None, swapChainDesc, out d3dDevice, out swapChain);

			//Getting device context
			d3dDeviceContext = d3dDevice.ImmediateContext;

			using (D3D11.Texture2D backBuffer = swapChain.GetBackBuffer<D3D11.Texture2D>(0))
			{
				renderTargetView = new D3D11.RenderTargetView(d3dDevice, backBuffer);
			}

		}

		private void Draw()
		{
			d3dDeviceContext.OutputMerger.SetRenderTargets(renderTargetView);
			d3dDeviceContext.ClearRenderTargetView(renderTargetView, new SharpDX.Color(32, 103, 178));
			swapChain.Present(1, PresentFlags.None);
		}

		//called every frame
		private void RenderCallback()
		{
			Draw();
		}


		//IDisposable, mustn't forgot
		public void Dispose()
		{
			renderTargetView.Dispose();
			swapChain.Dispose();
			d3dDevice.Dispose();
			d3dDeviceContext.Dispose();
			renderForm.Dispose();
		}
	}
}
