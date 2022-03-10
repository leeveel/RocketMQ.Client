﻿using System;

namespace RocketMQ.Client
{
    public class PullMessageService : ServiceThread
    {
        //private readonly InternalLogger log = ClientLogger.getLog();
        static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        //private readonly LinkedBlockingQueue<PullRequest> pullRequestQueue = new LinkedBlockingQueue<PullRequest>();
        private readonly MQClientInstance mQClientFactory;

        protected override Model ExecuteModel => Model.OnDemand;

        //private final ScheduledExecutorService scheduledExecutorService = Executors
        //.newSingleThreadScheduledExecutor(new ThreadFactory()
        //{
        //    @Override
        //    public Thread newThread(Runnable r)
        //    {
        //        return new Thread(r, "PullMessageServiceScheduledThread");
        //    }
        //});

        private readonly ScheduledExecutorService scheduledExecutorService;

        public PullMessageService(MQClientInstance mQClientFactory)
        {
            this.mQClientFactory = mQClientFactory;
            scheduledExecutorService = new ScheduledExecutorService();
        }

        public void executePullRequestLater(PullRequest pullRequest, long timeDelay)
        {
            if (!isStopped())
            {
                //    this.scheduledExecutorService.schedule(new Runnable() {
                //    @Override
                //    public void run()
                //    {
                //        PullMessageService.this.executePullRequestImmediately(pullRequest);
                //    }
                //}, timeDelay, TimeUnit.MILLISECONDS);
                scheduledExecutorService.Schedule(() => { executePullRequestImmediately(pullRequest); }, timeDelay);
            }
            else
            {
                log.Warn("PullMessageServiceScheduledThread has shutdown");
            }
        }

        public void executePullRequestImmediately(PullRequest pullRequest)
        {
            try
            {
                //this.pullRequestQueue.put(pullRequest);
                Executor.SendAsync(() => { pullMessage(pullRequest); });
            }
            catch (Exception e)
            {
                log.Error("executePullRequestImmediately pullRequestQueue.put", e.ToString());
            }
        }

        public void executeTaskLater(Runnable r, long timeDelay)
        {
            if (!isStopped())
            {
                //this.scheduledExecutorService.schedule(r, timeDelay, TimeUnit.MILLISECONDS);
                this.scheduledExecutorService.Schedule(r, timeDelay);
            }
            else
            {
                log.Warn("PullMessageServiceScheduledThread has shutdown");
            }
        }

        public ScheduledExecutorService getScheduledExecutorService()
        {
            return scheduledExecutorService;
        }

        private void pullMessage(PullRequest pullRequest)
        {
            MQConsumerInner consumer = this.mQClientFactory.selectConsumer(pullRequest.getConsumerGroup());
            if (consumer != null)
            {
                DefaultMQPushConsumerImpl impl = (DefaultMQPushConsumerImpl)consumer;
                impl.pullMessage(pullRequest);
            }
            else
            {
                log.Warn("No matched consumer for the PullRequest {}, drop it", pullRequest);
            }
        }

        public override void run() { throw new NotImplementedException(); }

        //public override void run()
        //{
        //    log.Info(this.getServiceName() + " service started");

        //    while (!this.isStopped())
        //    {
        //        try
        //        {
        //            PullRequest pullRequest = this.pullRequestQueue.take();
        //            this.pullMessage(pullRequest);
        //        }
        //        catch (ThreadInterruptedException ignored)
        //        {
        //        }
        //        catch (Exception e)
        //        {
        //            log.Error("Pull Message Service Run Method exception", e.ToString());
        //        }
        //    }

        //    log.Info(this.getServiceName() + " service end");
        //}

        //@Override
        public void shutdown(bool interrupt)
        {
            _ = base.Shutdown(interrupt);
            //ThreadUtils.shutdownGracefully(this.scheduledExecutorService, 1000, TimeUnit.MILLISECONDS);
        }

        //@Override
        public override string getServiceName()
        {
            return typeof(PullMessageService).Name;
            //return PullMessageService.class.getSimpleName();
        }

    }
}
